using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class HelpersManagementTests
    {
        private readonly Helper _testingHelper;
        private readonly IHelpersManagementService _helpersManagementService;
        private readonly IAccountManagementService _accountManagementService;
        private readonly JwtToken _token;

        public HelpersManagementTests()
        {
            _helpersManagementService = ServiceProvider.Instance.Resolve<IHelpersManagementService>();
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _helpersManagementService.RequestUri = RequestUriHelper.HelpersUri;
            _accountManagementService.RequestUri = RequestUriHelper.JwtTokenUri;
            _testingHelper = TestingObjectProvider.Instance.Helper;
            _token = _accountManagementService.SignIn(TestingObjectProvider.Instance.Login).Result;
        }

        [Fact]
        public void TestDependencyInjection()
        {
            Assert.NotNull(_helpersManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_helpersManagementService.RequestUri));
        }

        [Fact]
        public void TestOfGettingAllHelpers()
        {
            Assert.NotNull(_helpersManagementService.GetAllHelpers(_token.Jwt).Result);
        }

        [Fact]
        public void TestOfCreatingNewHelper()
        {
            var result = _helpersManagementService.CreateHelper(_testingHelper, _token.Jwt).Result;
            Assert.True(result);
        }

        [Fact]
        public void TestOfDeletingSpecifiedHelper()
        {
            Assert.True(_helpersManagementService.DeleteHelper(_testingHelper.Id, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfGettingSpecifiedHelper()
        {
            Assert.NotNull(_helpersManagementService.GetHelper(_testingHelper.Id, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfUpdatingHelper()
        {
            Assert.True(_helpersManagementService.UpdateHelper(_testingHelper, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfUpdatingHelperWithNullValue()
        {
            var exception = Record.Exception(() => _helpersManagementService.UpdateHelper(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllHelpersUsingWrongUrl()
        {
            _helpersManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_helpersManagementService.GetAllHelpers(_token.Jwt).Result);
            _helpersManagementService.RequestUri = RequestUriHelper.PetsUri;
        }

        [Fact]
        public void TestHelperValidationIfCorrectData()
        {
            var context = new ValidationContext(_testingHelper, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(_testingHelper, context, result, true);

            Assert.True(valid);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Surname")]
        public void TestUserValidationIfStringIsTooLong(string propertyName)
        {
            const int maxLength = 35;

            var helper = TestingObjectProvider.Instance.DoShallowCopy(_testingHelper);
            var property = helper.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            Assert.NotNull(property);

            property.SetValue(helper, new string('A', maxLength + 1));

            var context = new ValidationContext(helper, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(helper, context, result, true);

            Assert.False(valid);
        }

        [Theory]
        [InlineData("abcdabc.com")]
        [InlineData("test@test@.com")]
        [InlineData("http://abcd.com")]
        public void TestHelperEmailValidationIfNotCorrectData(string value)
        {
            var helper = TestingObjectProvider.Instance.DoShallowCopy(_testingHelper);
            helper.Email = value;

            var context = new ValidationContext(helper, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(helper, context, result, true);
            Assert.False(valid);
        }

        [Fact]
        public void TestHelperAddressValidation()
        {
            const int maxLength = 255;

            var helper = TestingObjectProvider.Instance.DoShallowCopy(_testingHelper);
            helper.Address = new string('a',maxLength+1);

            var context = new ValidationContext(helper, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(helper, context, result, true);

            Assert.False(valid);
        }

        [Fact]
        public void TestNotesValidation()
        {
            const int maxLength = 500;

            var helper = TestingObjectProvider.Instance.DoShallowCopy(_testingHelper);
            helper.Notes = new string('a', maxLength + 1);

            var context = new ValidationContext(helper, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(helper, context, result, true);

            Assert.False(valid);
        }
    }
}
