using Microsoft.Azure.ApplicationInsights.Query;
using DotNetAzureServicesCall.Services.ServiceInterface;

namespace DotNetAzureServicesCall.Services
{
    public class AzureAppInsightsService : IAzureAppInsightsService
    {
        public async Task<List<Dictionary<string, string>>> ValidateKQLFetchData(int minutes, string appInsightApplicationId, string appInsightAppKey, string operationName)
        {
            await Task.Delay(TimeSpan.FromMinutes(1));

            string query = $@"
                            requests
                            | where operation_Name contains ('{operationName}')
                            | where timestamp > ago({minutes}min)
                            | extend  timestamp, filter = parse_json(tostring(parse_json(customDimensions).trackedProperties)).CorrelationId
                            | where isnotnull (['filter'])
                            | extend  timestamp, rawLogs= parse_json(tostring(parse_json(customDimensions).trackedProperties))
                            | project timestamp, CorrelationId = rawLogs.CorrelationId, TransactionId = rawLogs.TransactionId, IntId= rawLogs.IntId, ActionName = rawLogs.ActionName, StatusCode = rawLogs.StatusCode, FileName = rawLogs.Filename, FileSize = rawLogs.Filesize
                            | extend isSuccessFlow = iff(isnotnull( IntId), true, false)
                            | where isSuccessFlow == true
                            | extend isSuccess = iff(StatusCode contains ""Ok"", true, false)
                            | sort by timestamp desc
                            ";

            var credentials = new ApiKeyClientCredentials(appInsightAppKey);
            var applicationInsightsClient = new ApplicationInsightsDataClient(credentials);

            var response = await applicationInsightsClient.Query.ExecuteWithHttpMessagesAsync(appInsightApplicationId, query);
            var results = response.Body.Results;
            if (response.Response.IsSuccessStatusCode && results != null)
            {
                var logs = results.Select(result => result.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty)).ToList();
                return logs;
            }

            throw new ApplicationException("No valid logs found in Application Insights response.");
        }
    }
}
