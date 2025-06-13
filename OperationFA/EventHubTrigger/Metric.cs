using System;
using System.Collections.Generic;
using System.Linq;
namespace EventHubTrigger
{
    class Metric
    {
        public string id { get; set; }
        public string metricName { get; set; }
        public string minimum { get; set; }
        public string maximum { get; set; }
        public string resourceId { get; set; }
    }
}
