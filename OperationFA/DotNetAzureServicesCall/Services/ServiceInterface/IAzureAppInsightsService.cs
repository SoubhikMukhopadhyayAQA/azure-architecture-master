namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface IAzureAppInsightsService
    {
        Task<List<Dictionary<string, string>>> ValidateKQLFetchData(int minutes, string appInsightApplicationId, string appInsightAppKey, string operationName);
    }
}
