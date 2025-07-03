using JetBrains.Annotations;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using DotNetAzureServicesCall.Services.ServiceInterface;
using System.Text;

namespace DotNetAzureServicesCall.Services
{
    [UsedImplicitly]
    public class AzureSbQueueService : IAzureSbQueueService
    {
        private readonly string _crmConnectionString;
        private readonly string _nexConnectionString;
        private readonly string _aidConnectionString;
        private readonly string _esbConnectionString;
        private readonly string _connectionString;

        public AzureSbQueueService(ConfigValues configValues)
        {
            _crmConnectionString = configValues.CrmSbQueueConnectionString ?? throw new ArgumentNullException(nameof(configValues.CrmSbQueueConnectionString));
            _nexConnectionString = configValues.NexSbQueueConnectionString ?? throw new ArgumentNullException(nameof(configValues.NexSbQueueConnectionString));
            _aidConnectionString = configValues.AidSbQueueConnectionString ?? throw new ArgumentNullException(nameof(configValues.AidSbQueueConnectionString));
            _esbConnectionString = configValues.EsbSbQueueConnectionString ?? throw new ArgumentNullException(nameof(configValues.EsbSbQueueConnectionString));
            _connectionString = configValues.AzureSbQueueConnectionString ?? throw new ArgumentNullException(nameof(configValues.AzureSbQueueConnectionString));
        }
        private string GetConnectionString(string sbFlag)
        {
            return sbFlag.ToLower() switch
            {
                "crm" => _crmConnectionString,
                "nex" => _nexConnectionString,
                "aid" => _aidConnectionString,
                "esb" => _esbConnectionString,
                _ => throw new ArgumentException($"Invalid service bus flag: {sbFlag}")
            };
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
        public async Task<bool> QueueExistsAsync(string queueName, string sbFlag)
        {
            try
            {
                var connectionString = GetConnectionString(sbFlag);
                var sbQueueClient = new ServiceBusAdministrationClient(connectionString);
                return await sbQueueClient.QueueExistsAsync(queueName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if queue exists: {ex.Message}");
                return false;
            }
        }
        public async Task SendMessagesToQueueAsync(string queueName, List<string> messages, string sbFlag)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));

            if (messages == null || messages.Count == 0)
                throw new ArgumentException("Messages list cannot be empty.", nameof(messages));
            try
            {
                var connectionString = GetConnectionString(sbFlag);
                if (await QueueExistsAsync(queueName, sbFlag))
                {
                    var client = new ServiceBusClient(connectionString);
                    var sender = client.CreateSender(queueName);

                    var messageBatch = await sender.CreateMessageBatchAsync();

                    foreach (var xmlContent in messages)
                    {

                        byte[] rawXmlMessage = Encoding.UTF8.GetBytes(xmlContent);

                        var message = new ServiceBusMessage(rawXmlMessage)
                        {
                            ContentType = "application/xml"
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

                    Console.WriteLine($"{messages.Count} messages sent to queue: {queueName}");
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
        public async Task<bool> CheckIfAssociateCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName, string sbFlag)
        {
            if (string.IsNullOrWhiteSpace(queueName)) throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));
            if (string.IsNullOrWhiteSpace(correlationIdPrefix)) throw new ArgumentException("Correlation-id prefix cannot be empty.", nameof(correlationIdPrefix));

            try
            {
                var connectionString = GetConnectionString(sbFlag);
                var client = new ServiceBusClient(connectionString);
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
        public async Task DeleteAssociateDetailsFromQueueIfCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName, string sbFlag)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));
            if (string.IsNullOrWhiteSpace(correlationIdPrefix))
                throw new ArgumentException("Correlation-id prefix cannot be empty.", nameof(correlationIdPrefix));

            var connectionString = GetConnectionString(sbFlag);
            await using var client = new ServiceBusClient(connectionString);
            await using var receiver = client.CreateReceiver(queueName);

            try
            {
                var receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 1000, maxWaitTime: TimeSpan.FromSeconds(5));

                foreach (var message in receivedMessages)
                {
                    var messageBody = message.Body;
                    if (!string.IsNullOrEmpty(messageBody.ToString()) && messageBody.ToString().Contains(correlationIdPrefix))
                    {
                        await receiver.CompleteMessageAsync(message);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking correlation ID prefix in queue '{queueName}'.", ex);
            }
        }
        public async Task SendJsonMessagesToQueueAsync(string queueName, List<string> messages, string sbFlag)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Queue name cannot be empty.", nameof(queueName));

            if (messages == null || messages.Count == 0)
                throw new ArgumentException("Messages list cannot be empty.", nameof(messages));

            try
            {
                var connectionString = GetConnectionString(sbFlag);
                if (await QueueExistsAsync(queueName, sbFlag))
                {
                    var client = new ServiceBusClient(connectionString);
                    var sender = client.CreateSender(queueName);

                    var messageBatch = await sender.CreateMessageBatchAsync();

                    foreach (var jsonContent in messages)
                    {
                        byte[] rawJsonMessage = Encoding.UTF8.GetBytes(jsonContent);

                        var message = new ServiceBusMessage(rawJsonMessage)
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

                    Console.WriteLine($"{messages.Count} JSON messages sent to queue: {queueName}");
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
        public async Task<bool> TopicExistsAsync(string topicName, string sbFlag)
        {
            try
            {
                var connectionString = GetConnectionString(sbFlag);
                var sbAdminClient = new ServiceBusAdministrationClient(connectionString);
                return await sbAdminClient.TopicExistsAsync(topicName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if topic exists: {ex.Message}");
                return false;
            }
        }
        public async Task SendJsonMessagesToTopicAsync(string topicName, List<string> messages, string sbFlag)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException("Topic name cannot be empty.", nameof(topicName));
            if (messages == null || messages.Count == 0)
                throw new ArgumentException("Messages list cannot be empty.", nameof(messages));
            try
            {
                var connectionString = GetConnectionString(sbFlag);
                if (await TopicExistsAsync(topicName, sbFlag))
                {
                    var client = new ServiceBusClient(connectionString);
                    var sender = client.CreateSender(topicName);
                    var messageBatch = await sender.CreateMessageBatchAsync();
                    foreach (var jsonContent in messages)
                    {
                        byte[] rawJsonMessage = Encoding.UTF8.GetBytes(jsonContent);
                        var message = new ServiceBusMessage(rawJsonMessage)
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
                    Console.WriteLine($"{messages.Count} JSON messages sent to topic: {topicName}");
                }
                else
                {
                    Console.WriteLine($"Topic '{topicName}' does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the message: {ex.Message}");
                throw;
            }
        }
    }
}
