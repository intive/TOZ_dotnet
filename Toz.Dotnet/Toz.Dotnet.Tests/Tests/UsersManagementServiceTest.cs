using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Tests.Tests
{
    public class UserManagementTest
    {
        private IUsersManagementService _UserManagementService;
        private User _testUser;
        public UserManagementTest()
        {
            _UserManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();
            _testUser = new User()
            {               
                Id = System.Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                Purpose = UserType.Volunteer
            };

            _UserManagementService.RequestUri = RequestUriHelper.UsersUri;
        }
        
        [Fact]
        public void TestDependencyInjectionFromUserManagementService()
        {
            Assert.NotNull(_UserManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_UserManagementService.RequestUri));
        }

        [Fact]
        public void TestOfGettingAllUsers()
        {
            Assert.NotNull(_UserManagementService.GetAllUsers().Result);
        }
         
        [Fact]
        public void TestOfCreatingNewUser()
        {
            Assert.True(_UserManagementService.CreateUser(_testUser).Result);
            _UserManagementService.DeleteUser(_testUser).Wait();
        }

        [Fact]
        public void TestOfDeletingSpecifiedUser()
        {
            _UserManagementService.CreateUser(_testUser);
            var User = _UserManagementService.GetAllUsers().Result;
            if(User.Any())
            {
                var firstUser = User.FirstOrDefault();
                Assert.False(_UserManagementService.CreateUser(firstUser).Result);
                Assert.True(_UserManagementService.DeleteUser(firstUser).Result);
                Assert.True(_UserManagementService.CreateUser(firstUser).Result);
            }
        }

        [Fact]
        public void TestOfGettingSpecifiedUser()
        {
            _UserManagementService.CreateUser(_testUser);
            var User = _UserManagementService.GetAllUsers().Result;
            if(User.Any())
            {
                var firstUser = User.FirstOrDefault();
                var singleUser = _UserManagementService.GetUser(firstUser.Id).Result;
                Assert.NotNull(singleUser);
            }
                       
            Assert.Null(_UserManagementService.GetUser("notExistingIDThatIsNotID--1").Result);           
        }

        
        [Fact]
        public void TestOfUserUpdating()
        {
            _UserManagementService.CreateUser(_testUser);
            var User = _UserManagementService.GetAllUsers().Result;
            if(User.Any())
            {
                var firstUser = User.FirstOrDefault();
                string userFirstName = firstUser.FirstName;
                firstUser.FirstName = "Test";
                Assert.True(_UserManagementService.UpdateUser(firstUser).Result);
                firstUser.FirstName = userFirstName;
                Assert.True(_UserManagementService.UpdateUser(firstUser).Result);
            }
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
                Purpose = User.Purpose
            };
        }
        
    }
}