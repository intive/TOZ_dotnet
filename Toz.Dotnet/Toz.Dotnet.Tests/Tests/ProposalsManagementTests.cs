using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class ProposalsManagementTests
    {
        private readonly Proposal _testingProposal;
        private readonly IProposalsManagementService _proposalsManagementService;

        public ProposalsManagementTests()
        {
            _proposalsManagementService = ServiceProvider.Instance.Resolve<IProposalsManagementService>();
            _proposalsManagementService.RequestUri = RequestUriHelper.ProposalsUri;
            _testingProposal = TestingObjectProvider.Instance.Proposal;
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
            Assert.NotNull(_proposalsManagementService.GetAllProposals().Result);
        }

        [Fact]
        public void TestOfCreatingNewProposal()
        {
            var result = _proposalsManagementService.CreateProposal(_testingProposal).Result;
            Assert.True(result);
        }

        [Fact]
        public void TestOfDeletingSpecifiedProposal()
        {
            Assert.True(_proposalsManagementService.DeleteProposal(_testingProposal).Result);
        }

        [Fact]
        public void TestOfUpdatingProposal()
        {
            Assert.True(_proposalsManagementService.UpdateProposal(_testingProposal).Result);
        }

        [Fact]
        public void TestOfUpdatingProposalWithNullValue()
        {
            var exception = Record.Exception(() => _proposalsManagementService.UpdateProposal(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfDeletingProposalThatIsNull()
        {
            var exception = Record.Exception(() => _proposalsManagementService.DeleteProposal(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllProposalsUsingWrongUrl()
        {
            _proposalsManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_proposalsManagementService.GetAllProposals().Result);
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
            const int maxLength = 30;

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
