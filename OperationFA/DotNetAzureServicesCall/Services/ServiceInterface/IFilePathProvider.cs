using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface IFilePathProvider
    {
        string GetFilePath(string subFolder, string fileName);
    }
}
