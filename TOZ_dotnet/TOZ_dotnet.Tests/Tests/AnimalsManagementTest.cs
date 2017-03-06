using Xunit;
using TOZ_dotnet.Core.Interfaces;
using TOZ_dotnet.Tests.Helpers;

namespace TOZ_dotnet.Tests.Tests
{
    public class AnimalsManagementTest
    {
        private IAnimalsManagementService _animalsManagementService;
        public AnimalsManagementTest()
        {
            _animalsManagementService = ServiceProvider.Instance.Resolve<IAnimalsManagementService>();
        }
        
        [Fact]
        public void TestDependencyInjectionFromAnimalsManagementService()
        {
            Assert.NotNull(_animalsManagementService);
            Assert.NotEmpty(_animalsManagementService.GetTestString());
        }
    }
}