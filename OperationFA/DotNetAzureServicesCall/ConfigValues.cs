using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAzureServicesCall
{
    public class ConfigValues
    {
        [UsedImplicitly] public string CrmSbQueueConnectionString { get; set; }
        [UsedImplicitly] public string NexSbQueueConnectionString { get; set; }
        [UsedImplicitly] public string EsbSbQueueConnectionString { get; set; }
        [UsedImplicitly] public string AidSbQueueConnectionString { get; set; }
        [UsedImplicitly] public string AzureSbQueueConnectionString { get; set; }
    }
}
