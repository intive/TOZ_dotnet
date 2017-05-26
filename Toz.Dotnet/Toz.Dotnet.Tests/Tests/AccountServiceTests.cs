using Moq;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class AccountServiceTests
    {
        private readonly IAccountManagementService _accountManagementService;
        private readonly Login _testingLogin;


        public AccountServiceTests()
        {
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _accountManagementService.RequestUri = RequestUriHelper.JwtTokenUri;
            _testingLogin = TestingObjectProvider.Instance.Login;
        }

        [Fact]
        public void TestDependencyInjectionFromAccountManagementService()
        {
            Assert.NotNull(_accountManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_accountManagementService.RequestUri));
        }

        [Fact]
        public void TestOfSignIn()
        {
            Assert.NotNull(_accountManagementService.SignIn(_testingLogin));
        }

        [Fact]
        public void TestOfSignInWithWrongUrl()
        {
            _accountManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_accountManagementService.SignIn(_testingLogin).Result);
            _accountManagementService.RequestUri = RequestUriHelper.JwtTokenUri;
        }

        [Fact]
        public void TestOfSignInWithNullLogin()
        {
            Assert.Null(_accountManagementService.SignIn(null).Result);
        }
    }
}
