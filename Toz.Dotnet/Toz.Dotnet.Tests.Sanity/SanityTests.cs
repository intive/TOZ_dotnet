using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace Toz.Dotnet.Tests.Sanity
{
    public class SanityTests
    {
        private IPetsManagementService _petsManagementService;
        private INewsManagementService _newsManagementService;
        private Pet _testingPet;
        private News _testingNews;
        public SanityTests()
        {
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();
            
            _testingPet = new Pet()
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = "TestDog",
                Type = PetType.DOG,
                Sex = PetSex.MALE,
                Photo = new byte[10],
                Description = "Dog that eats tigers",
                Address = "Found in the jungle",
                AddingTime = DateTime.Now,
                LastEditTime = DateTime.Now
            };

            _testingNews = new News()
            {               
                Id = System.Guid.NewGuid().ToString(),
                Title = "TestNews",
                PublishingTime = DateTime.Now,
                AddingTime = DateTime.Now,
                LastEditTime = DateTime.Now,
                Body = "Text",
                Photo = new byte[10],
                Status = NewsStatus.RELEASED
            };

            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
            _newsManagementService.RequestUri = RequestUriHelper.NewsUri;
        }
        

        [Fact]
        public void PetsFunctionalityTest()
        {
            Assert.True(!string.IsNullOrEmpty(_petsManagementService.RequestUri));
            //Get all pets
            var pets = _petsManagementService.GetAllPets().Result;
            Assert.NotNull(pets);
            //Get specified pet
            var firstPet = pets.FirstOrDefault();
            var pet = _petsManagementService.GetPet(firstPet.Id).Result;
            Assert.NotNull(pet);
            Assert.Null(_petsManagementService.GetPet("notExistingIDThatIsNotID--1").Result);
            //Create pet
            Assert.True(_petsManagementService.CreatePet(_testingPet).Result);
            //Delete pet
            Assert.True(_petsManagementService.DeletePet(_testingPet).Result);
            //Update pet
            string petName = firstPet.Name;
            firstPet.Name = "SanityTestName";
            Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
            firstPet.Name = petName;
            Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
        }

        [Fact]
        public void NewsFunctionalityTest()
        {
            Assert.True(!string.IsNullOrEmpty(_newsManagementService.RequestUri));
            //Get all news
            var news = _newsManagementService.GetAllNews().Result;
            Assert.NotNull(news);
            //Get specified news
            var firstNews = news.FirstOrDefault();
            var singleNews = _newsManagementService.GetNews(firstNews.Id).Result;
            Assert.NotNull(singleNews);
            Assert.Null(_newsManagementService.GetNews("notExistingIDThatIsNotID--1").Result);
            //Create news
            Assert.True(_newsManagementService.CreateNews(_testingNews).Result);
            //Delete news
            Assert.True(_newsManagementService.DeleteNews(_testingNews).Result);
            //Update news
            string newsTitle = firstNews.Title;
            firstNews.Title = "SanityTestTitle";
            Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
            firstNews.Title = newsTitle;
            Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
        }
    }
}
