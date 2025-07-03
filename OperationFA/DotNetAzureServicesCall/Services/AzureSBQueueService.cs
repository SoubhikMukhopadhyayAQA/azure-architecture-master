using JetBrains.Annotations;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using DotNetAzureServicesCall.Services.ServiceInterface;

namespace DotNetAzureServicesCall.Services
{
    [UsedImplicitly]
    public class AzureSbQueueService : IAzureSbQueueService
    {
        private readonly string _connectionString;
        // ReSharper disable once ConvertToPrimaryConstructor
        public AzureSbQueueService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<bool> QueueExistsAsync(string queueName)
        {
            try
            {
                var sbQueueClient = new ServiceBusAdministrationClient(_connectionString);
                return await sbQueueClient.QueueExistsAsync(queueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if queue exists: {ex.Message}");
                return false;
            }
        }

        public async Task SendJsonToQueueAsync(string queueName, List<string> jsonMessages)
        {
            if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));
            if (jsonMessages == null || jsonMessages.Count == 0) throw new ArgumentException("Messages list cannot be empty.", nameof(jsonMessages));

            try
            {
                if (await QueueExistsAsync(queueName))
                {
                    var client = new ServiceBusClient(_connectionString);
                    var sender = client.CreateSender(queueName);

                    var messageBatch = await sender.CreateMessageBatchAsync();

                    foreach (var jsonMessage in jsonMessages)
                    {
                        var message = new ServiceBusMessage(jsonMessage)
                        {
                            ContentType = "application/json"
                        };

                        if (!messageBatch.TryAddMessage(message))
                        {
                            await sender.SendMessagesAsync(messageBatch);
                            messageBatch = await sender.CreateMessageBatchAsync();
                            messageBatch.TryAddMessage(message);
                        }
                    }

                    if (messageBatch.Count > 0)
                    {
                        await sender.SendMessagesAsync(messageBatch);
                    }

                    Console.WriteLine($"{jsonMessages.Count} messages sent to queue: {queueName}");
                }
                else
                {
                    Console.WriteLine($"Queue '{queueName}' does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the message: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> CheckIfLearnerUlnPrefixPresentAsync(string correlationIdPrefix, string queueName)
        {
            if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));
            if (string.IsNullOrWhiteSpace(correlationIdPrefix)) throw new ArgumentException("ULN prefix cannot be empty.", nameof(correlationIdPrefix));

            try
            {
                var client = new ServiceBusClient(_connectionString);
                var receiver = client.CreateReceiver(queueName);

                var receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 10, maxWaitTime: TimeSpan.FromSeconds(5));

                foreach (var message in receivedMessages)
                {
                    var messageBody = message.Body.ToString();
                    if (messageBody.Contains(correlationIdPrefix))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while checking messages in the queue '{queueName}': {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CheckIfLearnerCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName)
        {
            if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));
            if (string.IsNullOrWhiteSpace(correlationIdPrefix)) throw new ArgumentException("Correlation-id prefix cannot be empty.", nameof(correlationIdPrefix));

            try
            {
                var client = new ServiceBusClient(_connectionString);
                var receiver = client.CreateReceiver(queueName);

                var receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 1000, maxWaitTime: TimeSpan.FromSeconds(5));

                foreach (var message in receivedMessages)
                {

                    var messageBody = message.Body.ToString();
                    if (messageBody.Contains(correlationIdPrefix))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while checking messages in the queue '{queueName}': {ex.Message}");
                throw;
            }
        }

        public async Task DeleteLearnerDetailsFromQueueIfCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));
            if (string.IsNullOrWhiteSpace(correlationIdPrefix))
                throw new ArgumentException("Correlation-id prefix cannot be empty.", nameof(correlationIdPrefix));

            try
            {
                var client = new ServiceBusClient(_connectionString);
                var receiver = client.CreateReceiver(queueName);

                var hasMoreMessages = true;

                while (hasMoreMessages)
                {
                    var receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 50, maxWaitTime: TimeSpan.FromSeconds(1));

                    if (receivedMessages.Count == 0)
                    {
                        hasMoreMessages = false;
                        continue;
                    }

                    var completeTasks = new List<Task>();

                    foreach (var message in receivedMessages)
                    {
                        if (message.CorrelationId != null && message.CorrelationId.StartsWith(correlationIdPrefix))
                        {
                            completeTasks.Add(receiver.CompleteMessageAsync(message));
                        }
                    }

                    if (completeTasks.Any())
                    {
                        await Task.WhenAll(completeTasks);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while processing the messages.", ex);
            }
        }
    }
}
