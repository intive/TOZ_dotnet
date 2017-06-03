using System;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class PetsStatusManagementTest
    {
        private readonly PetsStatus _testingStatus;
        private readonly IPetsStatusManagementService _petsStatusManagementService;
        private readonly IAccountManagementService _accountManagementService;
        private readonly JwtToken _token;

        public PetsStatusManagementTest()
        {
            _petsStatusManagementService = ServiceProvider.Instance.Resolve<IPetsStatusManagementService>();
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _petsStatusManagementService.RequestUri = RequestUriHelper.PetsStatusUri;
            _accountManagementService.RequestUri = RequestUriHelper.JwtTokenUri;
            _testingStatus = TestingObjectProvider.Instance.PetsStatus;
            _token = _accountManagementService.SignIn(TestingObjectProvider.Instance.Login).Result;
        }

        [Fact]
        public void TestDependencyInjection()
        {
            Assert.NotNull(_petsStatusManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_petsStatusManagementService.RequestUri));
        }

        [Fact]
        public void TestOfGettingAllPetsStatus()
        {
            Assert.NotNull(_petsStatusManagementService.GetAllStatus(_token.Jwt).Result);
        }

        [Fact]
        public void TestOfCreatingNewPetsStatus()
        {
            Assert.True(_petsStatusManagementService.CreateStatus(_testingStatus, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfDeletingSpecifiedPetsStatus()
        {
            Assert.True(_petsStatusManagementService.DeleteStatus(_testingStatus, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfUpdatingPetsStatus()
        {
            Assert.True(_petsStatusManagementService.UpdateStatus(_testingStatus, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfGettingSpecifiedPetsStatus()
        {
            Assert.NotNull(_petsStatusManagementService.GetStatus(_testingStatus.Id, _token.Jwt));
        }

        [Fact]
        public void TestOfUpdatingPetStatusWithNullValue()
        {
            var exception = Record.Exception(() => _petsStatusManagementService.UpdateStatus(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfDeletingPetStatusThatIsNull()
        {
            var exception = Record.Exception(() => _petsStatusManagementService.DeleteStatus(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllPetsStatusUsingWrongUrl()
        {
            _petsStatusManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_petsStatusManagementService.GetAllStatus(_token.Jwt).Result);
            _petsStatusManagementService.RequestUri = RequestUriHelper.PetsUri;
        }

        [Fact]
        public void TestOfCreatingNewPetsStatusWithNullValue()
        {
            Assert.False(_petsStatusManagementService.CreateStatus(null, _token.Jwt).Result);
        }
    }
}
