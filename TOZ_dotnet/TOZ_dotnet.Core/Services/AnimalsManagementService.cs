using TOZ_dotnet.Core.Interfaces;

namespace TOZ_dotnet.Core.Services
{
    public class AnimalsManagementService : IAnimalsManagementService
    {
        private IFilesManagementService _filesManagementService;
        public AnimalsManagementService(IFilesManagementService filesManagementService)
        {
            _filesManagementService = filesManagementService;
        }
        public string GetTestString()
        {
            return _filesManagementService.GetTestString();
        }
    }
}