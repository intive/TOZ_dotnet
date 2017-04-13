using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Linq;

namespace Toz.Dotnet.Tests.Sanity.Tests
{
    public class PetsManagementTest
    {
        private IPetsManagementService _petsManagementService;
        private Pet _testingPet;
        public PetsManagementTest()
        {
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
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

            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
        }

        [Fact]
        public void RequestedUriNotEmptyTest()
        {
            Assert.True(!string.IsNullOrEmpty(_petsManagementService.RequestUri));
        }

        [Fact]
        public void GetAllPetsTest()
        {
            Assert.NotNull(_petsManagementService.GetAllPets().Result);
        }
         
        [Fact]
        public void CreateNewPetTest()
        {
            bool created = _petsManagementService.CreatePet(_testingPet).Result;
            Assert.True(created);
            if(created)
            {
                _petsManagementService.DeletePet(_testingPet).Wait();
            }
            // var pets = _petsManagementService.GetAllPets().Result;
            // var randomPet = pets[1];
            // randomPet.Id = null;
            // randomPet.Name = "TestPet";
            // bool created = _petsManagementService.CreatePet(randomPet).Result;
            // Assert.True(created);
            // if(created)
            // {
            //     _petsManagementService.DeletePet(randomPet).Wait();
            // }
        }

        [Fact]
        public void DeleteSpecifiedPetTest()
        {
            var pets = _petsManagementService.GetAllPets().Result;
            if(pets.Any())
            {
                var firstPet = pets.FirstOrDefault();
                bool deleted = _petsManagementService.DeletePet(firstPet).Result;
                Assert.True(deleted);
                if(deleted)
                {
                    _petsManagementService.CreatePet(firstPet).Wait();
                }
            }
        }

        [Fact]
        public void GetSpecifiedPetTest()
        {
            var pets = _petsManagementService.GetAllPets().Result;
            if(pets.Any())
            {
                var firstPet = pets.FirstOrDefault();
                var pet = _petsManagementService.GetPet(firstPet.Id).Result;
                Assert.NotNull(pet);
            }
                       
            Assert.Null(_petsManagementService.GetPet("notExistingIDThatIsNotID--1").Result);           
        }

        [Fact]
        public void UpdatePetTest()
        {
            var pets = _petsManagementService.GetAllPets().Result;
            if(pets.Any())
            {
                var firstPet = pets.FirstOrDefault();
                string petName = firstPet.Name;
                firstPet.Name = "Test";
                Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
                firstPet.Name = petName;
                Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
            }
        }
    }
}