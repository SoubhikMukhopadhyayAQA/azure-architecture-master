using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text;
using Azure.Messaging.EventHubs;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Cosmos.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace EventHubTrigger
{
    public class ProcessMessages
    {
        private static readonly string _connection_string = "";
        private static readonly string _database_name = "logdb";
        private static readonly string _container_name = "dbmetrics";

        [Function(nameof(ProcessMessages))]
        public static async Task Run([EventHubTrigger("sqlhub", Connection = "hubconnection")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            CosmosClient _cosmosclient = new CosmosClient(_connection_string, new CosmosClientOptions());
            Microsoft.Azure.Cosmos.Container _container = _cosmosclient.GetContainer(_database_name, _container_name);

            foreach (EventData eventData in events)
            {
                try
                {
                    // Fix for CS1503: Convert ReadOnlyMemory<byte> to byte[] using ToArray()  
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.ToArray());
                    JObject records = JObject.Parse(messageBody);

                    JArray jArray = JArray.Parse(records["records"].ToString());
                    var jObjects = jArray.ToObject<List<JObject>>();
                    foreach (var obj in jObjects)                             //Loop through on a list  
                    {
                        Metric metric = new Metric();

                        metric.metricName = obj["metricName"].ToString();
                        metric.minimum = obj["minimum"].ToString();
                        metric.maximum = obj["maximum"].ToString();
                        metric.resourceId = obj["resourceId"].ToString();
                        metric.id = Guid.NewGuid().ToString();
                        _container.CreateItemAsync(metric, new PartitionKey(metric.metricName)).GetAwaiter().GetResult();
                    }

                    log.LogInformation("Added database metric");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.  
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.  
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.  

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
