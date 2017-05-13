using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Tests.Tests
{
    public class UserManagementTest
    {
        private readonly IUsersManagementService _userManagementService;
        private User _testUser;
        public UserManagementTest()
        {
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();

            _testUser = new User
            {               
                Id = System.Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                //Roles = UserType.Volunteer
            };

            _userManagementService.RequestUri = RequestUriHelper.UsersUri;
        }
        
        /*[Fact]
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
        public void TestOfGettingAllUsers()
        {
            Assert.NotNull(_userManagementService.GetAllUsers().Result);
        }
         
        [Fact]
        public void TestOfCreatingNewUser()
        {
            Assert.True(_userManagementService.CreateUser(_testUser).Result);
            _userManagementService.DeleteUser(_testUser).Wait();
        }

        [Fact]
        public void TestOfDeletingSpecifiedUser()
        {
            _userManagementService.CreateUser(_testUser);
            var User = _userManagementService.GetAllUsers().Result;
            if(User.Any())
            {
                var firstUser = User.FirstOrDefault();
                Assert.False(_userManagementService.CreateUser(firstUser).Result);
                Assert.True(_userManagementService.DeleteUser(firstUser).Result);
                Assert.True(_userManagementService.CreateUser(firstUser).Result);
            }
        }

        [Fact]
        public void TestOfGettingSpecifiedUser()
        {
            _userManagementService.CreateUser(_testUser);
            var User = _userManagementService.GetAllUsers().Result;
            if(User.Any())
            {
                var firstUser = User.FirstOrDefault();
                var singleUser = _userManagementService.GetUser(firstUser.Id).Result;
                Assert.NotNull(singleUser);
            }
                       
            Assert.Null(_userManagementService.GetUser("notExistingIDThatIsNotID--1").Result);           
        }

        
        [Fact]
        public void TestOfUserUpdating()
        {
            _userManagementService.CreateUser(_testUser);
            var User = _userManagementService.GetAllUsers().Result;
            if(User.Any())
            {
                var firstUser = User.FirstOrDefault();
                string userFirstName = firstUser.FirstName;
                firstUser.FirstName = "Test";
                Assert.True(_userManagementService.UpdateUser(firstUser).Result);
                firstUser.FirstName = userFirstName;
                Assert.True(_userManagementService.UpdateUser(firstUser).Result);
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
        }*/
        
    }
}
