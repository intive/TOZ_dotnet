using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;

namespace Toz.Dotnet.Tests.Tests
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
        }
    }
}