using TOZ_dotnet.Core.Interfaces;

namespace TOZ_dotnet.Core.Services
{
    public class PetsManagementService : IPetsManagementService
    {
        private IFilesManagementService _filesManagementService;
        public PetsManagementService(IFilesManagementService filesManagementService)
        {
            _filesManagementService = filesManagementService;
        }
        public string GetTestString()
        {
            return _filesManagementService.GetTestString();
        }
    }
}