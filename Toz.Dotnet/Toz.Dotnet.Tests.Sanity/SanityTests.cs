using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Linq;

namespace Toz.Dotnet.Tests.Sanity
{
    public class SanityTests
    {
        private IPetsManagementService _petsManagementService;
        private INewsManagementService _newsManagementService;
        private IUsersManagementService _userManagementService;
        private Pet _testPet;
        private News _testNews;
        private User _testUser;
        public SanityTests()
        {
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();

            _testPet = new Pet()
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = "SanityTestDog",
                Type = PetType.DOG,
                Sex = PetSex.MALE,
                Photo = new byte[10],
                Description = "Dog that eats tigers",
                Address = "Found in the jungle",
                AddingTime = DateTime.Now,
                LastEditTime = DateTime.Now
            };

            _testNews = new News()
            {               
                Id = System.Guid.NewGuid().ToString(),
                Title = "SanityTestNews",
                PublishingTime = DateTime.Now,
                AddingTime = DateTime.Now,
                LastEditTime = DateTime.Now,
                Body = "Text",
                Photo = new byte[10],
                Status = NewsStatus.RELEASED
            };

            _testUser = new User()
            {
                Id = System.Guid.NewGuid().ToString(),
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                Purpose = UserType.Volunteer
            };

            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
            _newsManagementService.RequestUri = RequestUriHelper.NewsUri;
            _userManagementService.RequestUri = RequestUriHelper.UsersUri;
        }
        

        [Fact]
        public void PetsFunctionalityTest()
        {
            Assert.True(!string.IsNullOrEmpty(_petsManagementService.RequestUri));
            //Get all pets
            var pets = _petsManagementService.GetAllPets().Result;
            Assert.NotNull(pets);
            //Get specified pet
            if(pets.Any())
            {
                var firstPet = pets.FirstOrDefault();
                var pet = _petsManagementService.GetPet(firstPet.Id).Result;
                Assert.NotNull(pet);
                Assert.Null(_petsManagementService.GetPet("notExistingIDThatIsNotID--1").Result);
                //Update pet
                string petName = firstPet.Name;
                firstPet.Name = "SanityTestName";
                Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
                firstPet.Name = petName;
                Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
            }
            //Create pet
            Assert.True(_petsManagementService.CreatePet(_testPet).Result);
            //Delete pet
            Assert.True(_petsManagementService.DeletePet(_testPet).Result);
        }

        [Fact]
        public void NewsFunctionalityTest()
        {
            Assert.True(!string.IsNullOrEmpty(_newsManagementService.RequestUri));
            //Get all news
            var news = _newsManagementService.GetAllNews().Result;
            Assert.NotNull(news);
            //Get specified news
            if(news.Any())
            {
                var firstNews = news.FirstOrDefault();
                var singleNews = _newsManagementService.GetNews(firstNews.Id).Result;
                Assert.NotNull(singleNews);
                Assert.Null(_newsManagementService.GetNews("notExistingIDThatIsNotID--1").Result);
                //Update news
                string newsTitle = firstNews.Title;
                firstNews.Title = "SanityTestTitle";
                Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
                firstNews.Title = newsTitle;
                Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
            }
            //Create news
            Assert.True(_newsManagementService.CreateNews(_testNews).Result);
            //Delete news
            Assert.True(_newsManagementService.DeleteNews(_testNews).Result);
        }

        [Fact]
        public void UsersFunctionalityTest()
        {
            Assert.True(!string.IsNullOrEmpty(_userManagementService.RequestUri));
            //Get all news
            var users = _userManagementService.GetAllUsers().Result;
            Assert.NotNull(users);
            //Get specified news
            if(users.Any())
            {
                var firstUser = users.FirstOrDefault();
                var singleUser = _userManagementService.GetUser(firstUser.Id).Result;
                Assert.NotNull(singleUser);
                Assert.Null(_userManagementService.GetUser("notExistingIDThatIsNotID--1").Result);
                //Update news
                string userFirsName = firstUser.FirstName;
                firstUser.FirstName = "SanityTestFirstName";
                Assert.True(_userManagementService.UpdateUser(firstUser).Result);
                firstUser.FirstName = userFirsName;
                Assert.True(_userManagementService.UpdateUser(firstUser).Result);
            }
            //Create news
            Assert.True(_userManagementService.CreateUser(_testUser).Result);
            //Delete news
            Assert.True(_userManagementService.DeleteUser(_testUser).Result);
        }
    }
}
