using Xunit;
using TOZ_dotnet.Core.Interfaces;
using TOZ_dotnet.Tests.Helpers;

namespace TOZ_dotnet.Tests.Tests
{
    public class PetsManagementTest
    {
        private IPetsManagementService _animalsManagementService;
        public PetsManagementTest()
        {
            _animalsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
        }
        
        [Fact]
        public void TestDependencyInjectionFromAnimalsManagementService()
        {
            Assert.NotNull(_animalsManagementService);
            Assert.NotEmpty(_animalsManagementService.GetTestString());
        }
    }
}