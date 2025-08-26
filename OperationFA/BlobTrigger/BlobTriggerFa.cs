using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlobTrigger;

public class BlobTriggerFa
{
    private readonly ILogger<BlobTriggerFa> _logger;  

    public BlobTriggerFa(ILogger<BlobTriggerFa> logger) 
    {
        _logger = logger;
    }

    [Function(nameof(BlobTriggerFa))]
    public async Task Run(
        [BlobTrigger("loadtestcontainer/{name}", Connection = "MyStorageConnection")] Stream blobStream,
        string name)
    {
        try
        {
            _logger.LogInformation("Blob trigger function processed blob\n Name: {name} \n Size: {size} Bytes", name, blobStream.Length);
            using var reader = new StreamReader(blobStream);
            string messageText = await reader.ReadToEndAsync();

            string serviceBusConnectionString = Environment.GetEnvironmentVariable("MyServiceBusConnection");
            string queueName = "aqa-esb-sit-02-euw-bus-apps-que-out-lnrach";

            await using var client = new ServiceBusClient(serviceBusConnectionString);
            ServiceBusSender sender = client.CreateSender(queueName);

            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageText))
            {
                ContentType = "JSON"
            };

            await sender.SendMessageAsync(serviceBusMessage);

            _logger.LogInformation("Message from blob '{name}' sent to Service Bus queue: {queueName}", name, queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing blob or forwarding message to Service Bus.");
            throw;
        }
    }
}
