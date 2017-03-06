using System;
using TOZ_dotnet.Core.Interfaces;

namespace TOZ_dotnet.Core.Services
{
    public class FilesManagementService : IFilesManagementService
    {
        public string GetTestString()
        {
            return "This is testing string from the FilesManagementService";
        }
    }
}