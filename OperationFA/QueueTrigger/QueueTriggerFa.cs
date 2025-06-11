using System.Text;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace QueueTrigger;

public class QueueTriggerFa
{
    private readonly ILogger<QueueTriggerFa> _logger;

    public QueueTriggerFa(ILogger<QueueTriggerFa> logger)
    {
        _logger = logger;
    }

    [Function(nameof(QueueTriggerFa))]
    public async Task Run(
        [QueueTrigger("loadtestinputqueue", Connection = "MyStorageConnection")] QueueMessage message)
    {
        //_logger.LogInformation("Queue message received: {messageText}", message.MessageText);

        try
        {
            string serviceBusConnectionString = Environment.GetEnvironmentVariable("MyServiceBusConnection");
            string queueName = "aqa-esb-sit-02-euw-bus-apps-que-in-lnrach";

            await using var client = new ServiceBusClient(serviceBusConnectionString);
            ServiceBusSender sender = client.CreateSender(queueName);

            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message.MessageText))
            {
                ContentType = "JSON"
            };

            await sender.SendMessageAsync(serviceBusMessage);

            //_logger.LogInformation("Plain message sent to Service Bus queue: {queueName}", queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error forwarding message to Service Bus.");
            throw;
        }
    }
}