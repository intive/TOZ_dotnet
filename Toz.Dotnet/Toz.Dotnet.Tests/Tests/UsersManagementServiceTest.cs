using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class UserManagementTest
    {
        private readonly AuthService _authHelper;
        private readonly IUsersManagementService _userManagementService;
        private User _testUser;

        private const string EmailBaseValue = "test";
        private const string EmailDomainValue = "test.com";
        public UserManagementTest()
        {
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();
            _authHelper = new AuthService();
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

        private string CreateNewMail(string baseValue, int number, string domain)
        {
            return $"{baseValue}{number}@{domain}";
        }

        private async Task<bool> UserAlreadyExists(User user)
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            var users = await _userManagementService.GetAllUsers();
            return users.Any(usr => usr.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase));
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
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            Assert.NotNull(await _userManagementService.GetAllUsers());
        }
         
        [Fact]
        public async void TestOfCreatingNewUser()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            var currentEmailLevel = 0;
            while (await UserAlreadyExists(_testUser))
            {
                _testUser.Email = CreateNewMail(EmailBaseValue, currentEmailLevel, EmailDomainValue);
                currentEmailLevel++;
            }
            Assert.True(await _userManagementService.CreateUser(_testUser));
            await _userManagementService.DeleteUser(_testUser);
        }

        [Fact]
        public async void TestOfGettingSpecifiedUser()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            var currentEmailLevel = 0;
            while (await UserAlreadyExists(_testUser))
            {
                _testUser.Email = CreateNewMail(EmailBaseValue, currentEmailLevel, EmailDomainValue);
                currentEmailLevel++;
            }
            await _userManagementService.CreateUser(_testUser);
            var users = _userManagementService.GetAllUsers().Result;
            if(users.Any())
            {
                var firstUser = users.FirstOrDefault();
                var singleUser = _userManagementService.GetUser(firstUser.Id).Result;
                Assert.NotNull(singleUser);
            }
                       
            Assert.Null(_userManagementService.GetUser("notExistingIDThatIsNotID--1").Result);
            await _userManagementService.DeleteUser(_testUser);
        }

        [Fact]
        public async void TestOfUserUpdating()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            var currentEmailLevel = 0;
            while (await UserAlreadyExists(_testUser))
            {
                _testUser.Email = CreateNewMail(EmailBaseValue, currentEmailLevel, EmailDomainValue);
                currentEmailLevel++;
            }
            var creationResult = await _userManagementService.CreateUser(_testUser);
            if (!creationResult)
            {
                return;
            }
            var users = await _userManagementService.GetAllUsers();
            if(users != null && users.Any())
            {
                var testedUser = users.FirstOrDefault(u=> u.Email == _testUser.Email);
                Assert.NotNull(testedUser);

                testedUser.FirstName = "Test";
                testedUser.Password = "TestPasswd";
                Assert.True(await _userManagementService.UpdateUser(testedUser));
            }
            await _userManagementService.DeleteUser(_testUser);
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

        private User CloneUser(User User)
        {
            return new User()
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                PhoneNumber = User.PhoneNumber,
                Email = User.Email,
                Roles = User.Roles
            };
        }
        
    }
}
