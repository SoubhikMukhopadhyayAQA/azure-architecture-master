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
    }
}
