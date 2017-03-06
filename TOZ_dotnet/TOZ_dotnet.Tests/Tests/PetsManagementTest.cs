using Xunit;
using TOZ_dotnet.Core.Interfaces;
using TOZ_dotnet.Tests.Helpers;

namespace TOZ_dotnet.Tests.Tests
{
    public class PetsManagementTest
    {
        private IPetsManagementService _petsManagementService;
        public PetsManagementTest()
        {
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
        }
        
        [Fact]
        public void TestDependencyInjectionFromPetsManagementService()
        {
            Assert.NotNull(_petsManagementService);
            Assert.NotEmpty(_petsManagementService.GetTestString());
        }
    }
}