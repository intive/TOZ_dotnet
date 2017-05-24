using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class UserManagementTest
    {
        private readonly IUsersManagementService _userManagementService;
        private readonly User _testUser;
        private readonly IAccountManagementService _accountManagementService;
        private readonly JwtToken _token;

        public UserManagementTest()
        {
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _userManagementService.RequestUri = RequestUriHelper.UsersUri;
            _accountManagementService.RequestUri = RequestUriHelper.JwtTokenUrl;
            _testUser = TestingObjectProvider.Instance.User;
            _token = _accountManagementService.SignIn(TestingObjectProvider.Instance.Login).Result;
        }

        [Fact]
        public void TestDependencyInjectionFromUserManagementService()
        {
            Assert.NotNull(_userManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_userManagementService.RequestUri));
        }

        [Fact]
        public async void TestOfGettingAllUsers()
        {
            Assert.NotNull(await _userManagementService.GetAllUsers(_token.Jwt));
        }
         
        [Fact]
        public async void TestOfCreatingNewUser()
        {
            Assert.True(await _userManagementService.CreateUser(_testUser, _token.Jwt));
        }

        [Fact]
        public async void TestOfGettingSpecifiedUser()
        {
            Assert.True(await _userManagementService.DeleteUser(_testUser, _token.Jwt));
        }

        [Fact]
        public async void TestOfUserUpdating()
        {
            Assert.True(await _userManagementService.UpdateUser(_testUser, _token.Jwt));
        }

        [Fact]
        public void TestOfUpdatingPetWithNullValue()
        {
            var exception = Record.Exception(() => _userManagementService.UpdateUser(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfDeletingPetThatIsNull()
        {
            var exception = Record.Exception(() => _userManagementService.DeleteUser(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllPetsUsingWrongUrl()
        {
            _userManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_userManagementService.GetAllUsers(_token.Jwt).Result);
            _userManagementService.RequestUri = RequestUriHelper.PetsUri;
        }

        [Fact]
        public void TestUserValidationIfCorrectData()
        {
            // Arrange
            var context = new ValidationContext(_testUser, null, null);
            var result = new List<ValidationResult>();

            // Act
            bool valid = Validator.TryValidateObject(_testUser, context, result, true);

            Assert.True(valid);
        }

        [Theory]
        [InlineData("+48123123123")]
        [InlineData("123456789")]
        public void TestUserPhoneValidationIfCorrectData(string value)
        {
            var user = TestingObjectProvider.Instance.DoShallowCopy(_testUser);
            user.PhoneNumber = value;

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("abcd@abc.com")]
        [InlineData("test@test.com")]
        public void TestUserEmailValidationIfCorrectData(string value)
        {
            var user = TestingObjectProvider.Instance.DoShallowCopy(_testUser);
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
        public void TestUserEmailValidationIfNotCorrectData(string value)
        {
            var user = TestingObjectProvider.Instance.DoShallowCopy(_testUser);
            user.Email = value;

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);
            Assert.False(valid);
        }

        [Theory]
        [InlineData("FirstName")]
        [InlineData("LastName")]
        public void TestUserValidationIfStringIsTooLong(string propertyName)
        {
            const int maxLength = 30;

            var user = TestingObjectProvider.Instance.DoShallowCopy(_testUser);
            var property = user.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            Assert.NotNull(property);

            property.SetValue(user,new string('A',maxLength+1));

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);

            Assert.False(valid);
        }
       
    }
}
