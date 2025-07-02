using System.Reflection;
using DotNetAzureServicesCall.Services.ServiceInterface;

namespace DotNetAzureServicesCall.Services
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetFilePath(string subFolder, string fileName)
        {
            var codeBaseUri = new Uri(Assembly.GetExecutingAssembly().Location);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUri.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);

            var projectDirectory = SearchForProjectDirectory(dirPath);
            return Path.Combine(projectDirectory, Constants.TestDataFolder, subFolder, fileName);
        }

        private string SearchForProjectDirectory(string currentDirectory)
        {
            while (true)
            {
                var projectFiles = Directory.GetFiles(currentDirectory, "*.csproj");
                if (projectFiles.Any())
                {
                    return currentDirectory;
                }
                var parentDirectory = Directory.GetParent(currentDirectory);
                if (parentDirectory == null)
                {
                    throw new Exception("Project directory not found.");
                }
                currentDirectory = parentDirectory.FullName;
            }
        }
    }
}
