using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Core.Services
{
    public class FilesManagementService : IFilesManagementService
    {
        public string GetTestString()
        {
            return "This is testing string from the FilesManagementService";
        }
    }
}