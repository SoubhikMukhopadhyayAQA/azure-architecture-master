using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface IAzureSbQueueService
    {
        Task<bool> QueueExistsAsync(string queueName);
        Task SendJsonToQueueAsync(string queueName, List<string> jsonMessages);
        Task<bool> CheckIfLearnerUlnPrefixPresentAsync(string ulnPrefix, string queueName);
        Task<bool> CheckIfLearnerCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName);
        Task DeleteLearnerDetailsFromQueueIfCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName);
        Task<bool> QueueExistsAsync(string queueName, string sbFlag);
        Task SendMessagesToQueueAsync(string queueName, List<string> messages, string sbFlag);
        Task<bool> CheckIfAssociateCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName, string sbFlag);
        Task DeleteAssociateDetailsFromQueueIfCorrelationIdPrefixPresentAsync(string correlationIdPrefix, string queueName, string sbFlag);
        Task SendJsonMessagesToQueueAsync(string queueName, List<string> messages, string sbFlag);
        Task<bool> TopicExistsAsync(string topicName, string sbFlag);
        Task SendJsonMessagesToTopicAsync(string topicName, List<string> messages, string sbFlag);
    }
}
