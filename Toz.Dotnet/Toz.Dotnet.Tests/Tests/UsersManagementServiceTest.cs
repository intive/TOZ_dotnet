using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class UserManagementTest
    {
        private readonly IUsersManagementService _userManagementService;
        private readonly User _testUser;

        public UserManagementTest()
        {
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();
            _testUser = new User
            {               
                FirstName = "Mariusz",
                LastName = "Wolonatriusz",
                Password = "TajneHasloMariusza",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                Roles = new [] {UserType.Volunteer}
            };

            _userManagementService.RequestUri = RequestUriHelper.UsersUri;
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
            Assert.NotNull(await _userManagementService.GetAllUsers());
        }
         
        [Fact]
        public async void TestOfCreatingNewUser()
        {
            Assert.True(await _userManagementService.CreateUser(_testUser));
        }

        [Fact]
        public async void TestOfGettingSpecifiedUser()
        {
            Assert.True(await _userManagementService.DeleteUser(_testUser));
        }

        [Fact]
        public async void TestOfUserUpdating()
        {
            Assert.True(await _userManagementService.UpdateUser(_testUser));
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
            var user = CloneUser(_testUser);
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
            var user = CloneUser(_testUser);
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
            var user = CloneUser(_testUser);
            user.Email = value;

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);
            Assert.False(valid);
        }

        [Theory]
        [InlineData("FirstName")]
        [InlineData("LastName")]
        public void TestPetValidationIfStringIsTooLong(string propertyName)
        {
            const int maxLength = 30;

            var user = CloneUser(_testUser);
            var property = user.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            Assert.NotNull(property);

            property.SetValue(user,new string('A',maxLength+1));

            var context = new ValidationContext(user, null, null);
            var result = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(user, context, result, true);

            Assert.False(valid);
        }

        private User CloneUser(User user)
        {
            return new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Roles = user.Roles,
                Password = user.Password
            };
        }
        
    }
}
