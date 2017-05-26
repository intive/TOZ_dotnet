using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Core.Services;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class ProposalsManagementTests
    {
        private readonly Proposal _testingProposal;
        private readonly IProposalsManagementService _proposalsManagementService;
        private readonly IAccountManagementService _accountManagementService;
        private readonly JwtToken _token;

        public ProposalsManagementTests()
        {
            _proposalsManagementService = ServiceProvider.Instance.Resolve<IProposalsManagementService>();
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _proposalsManagementService.RequestUri = RequestUriHelper.ProposalsUri;
            _accountManagementService.RequestUri = RequestUriHelper.JwtTokenUri;
            _testingProposal = TestingObjectProvider.Instance.Proposal;
            _token = _accountManagementService.SignIn(TestingObjectProvider.Instance.Login).Result;
        }

        [Fact]
        public void TestDependencyInjection()
        {
            Assert.NotNull(_proposalsManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_proposalsManagementService.RequestUri));
        }

        [Fact]
        public void TestOfGettingAllProposals()
        {
            Assert.NotNull(_proposalsManagementService.GetAllProposals(_token.Jwt).Result);
        }

        [Fact]
        public void TestOfCreatingNewProposal()
        {
            var result = _proposalsManagementService.CreateProposal(_testingProposal, _token.Jwt).Result;
            Assert.True(result);
        }

        [Fact]
        public void TestOfDeletingSpecifiedProposal()
        {
            Assert.True(_proposalsManagementService.DeleteProposal(_testingProposal, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfUpdatingProposal()
        {
            Assert.True(_proposalsManagementService.UpdateProposal(_testingProposal, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfUpdatingProposalWithNullValue()
        {
            var exception = Record.Exception(() => _proposalsManagementService.UpdateProposal(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfDeletingProposalThatIsNull()
        {
            var exception = Record.Exception(() => _proposalsManagementService.DeleteProposal(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllProposalsUsingWrongUrl()
        {
            _proposalsManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_proposalsManagementService.GetAllProposals(_token.Jwt).Result);
            _proposalsManagementService.RequestUri = RequestUriHelper.PetsUri;
        }

        [Fact]
        public void TestProposalValidationIfCorrectData()
        {
            // Arrange
            var context = new ValidationContext(_testingProposal, null, null);
            var result = new List<ValidationResult>();

            // Act
            bool valid = Validator.TryValidateObject(_testingProposal, context, result, true);

            Assert.True(valid);
        }

        [Fact]
        public void TestOfGettingSpecifiedProposal()
        {
            var restServiceMock = new Mock<IRestService>();
            restServiceMock.Setup(s => s.ExecuteGetAction<List<Proposal>>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken> ()))
                .ReturnsAsync(new List<Proposal>()
                {
                    TestingObjectProvider.Instance.DoShallowCopy(_testingProposal)
                });

            var options = ServiceProvider.Instance.Resolve<IOptions<AppSettings>>();
            var proposalManagementService =
                new ProposalsManagementService(restServiceMock.Object, options)
                {
                    RequestUri = RequestUriHelper.ProposalsUri,
                    ActivationRequestUri = RequestUriHelper.ProposalsUri
                };

            Assert.NotNull(proposalManagementService.GetProposal(_testingProposal.Id, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfProposalActivation()
        {
            var restServiceMock = new Mock<IRestService>();
            restServiceMock.Setup(s => s.ExecuteGetAction<Proposal>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestingObjectProvider.Instance.DoShallowCopy(_testingProposal));

            restServiceMock.Setup(s => s.ExecutePutAction<Proposal>(
                    It.IsAny<string>(),
                    It.IsAny<Proposal>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var options = ServiceProvider.Instance.Resolve<IOptions<AppSettings>>();
            var proposalManagementService =
                new ProposalsManagementService(restServiceMock.Object, options)
                {
                    RequestUri = RequestUriHelper.ProposalsUri,
                    ActivationRequestUri = RequestUriHelper.ProposalsUri
                };
            Assert.NotNull(proposalManagementService.RequestUri);
            Assert.NotNull(proposalManagementService.ActivationRequestUri);
            Assert.True(proposalManagementService.SendActivationEmail(_testingProposal.Id, _token.Jwt).Result);
        }

        [Theory]
        [InlineData("+48123123123")]
        [InlineData("123456789")]
        public void TestProposalPhoneValidationIfCorrectData(string value)
        {
            var user = TestingObjectProvider.Instance.DoShallowCopy(_testingProposal);
            user.PhoneNumber = value;

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("abcd@abc.com")]
        [InlineData("test@test.com")]
        public void TestProposalEmailValidationIfCorrectData(string value)
        {
            var user = TestingObjectProvider.Instance.DoShallowCopy(_testingProposal);
            user.Email = value;

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("abcdabc.com")]
        [InlineData("test@test@.com")]
        [InlineData("http://abcd.com")]
        public void TestProposalEmailValidationIfNotCorrectData(string value)
        {
            var user = TestingObjectProvider.Instance.DoShallowCopy(_testingProposal);
            user.Email = value;

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);
            Assert.False(valid);
        }

        [Theory]
        [InlineData("FirstName")]
        [InlineData("LastName")]
        public void TestProposalValidationIfStringIsTooLong(string propertyName)
        {
            const int maxLength = 35;

            var user = TestingObjectProvider.Instance.DoShallowCopy(_testingProposal);
            var property = user.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            Assert.NotNull(property);

            property.SetValue(user, new string('A', maxLength + 1));

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);

            Assert.False(valid);
        }
    }
}
