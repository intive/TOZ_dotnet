using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;

namespace Toz.Dotnet.Tests.Tests
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
                Id = 9999,
                Name = "TestDog",
                Type = PetType.Dog,
                Sex = PetSex.Male,
                Photo = new byte[10],
                Description = "Dog that eats tigers",
                Address = "Found in jungle",
                AddingTime = DateTime.Now,
                LastEditTime = DateTime.Now
            };
        }
        
        [Fact]
        public void TestDependencyInjectionFromPetsManagementService()
        {
            Assert.NotNull(_petsManagementService);
        }

        [Fact]
        public void TestIfGetAllPetsIsNotNull()
        {
            Assert.NotNull(_petsManagementService.GetAllPets());
        }

        [Fact]
        public void TestNewPetAdding()
        {
            Assert.True(_petsManagementService.CreatePet(_testingPet));
            _petsManagementService.DeletePet(_testingPet);
        }

        [Fact]
        public void TestPetDeleting()
        {
            _petsManagementService.CreatePet(_testingPet);
            Assert.True(_petsManagementService.DeletePet(_testingPet));
        }

        [Fact]
        public void TestGetSpecifiedPet()
        {
            _petsManagementService.CreatePet(_testingPet);
            var pet = _petsManagementService.GetPet(_testingPet.Id);

            Assert.NotNull(pet);
            //Assert.Equal(_testingPet.Id, pet.Id);
        }

        
        [Fact]
        public void CheckPetUpdate()
        {
            _petsManagementService.CreatePet(_testingPet);
            var pet = _petsManagementService.GetPet(_testingPet.Id);
            pet.Name = "UpdatedName";
            Assert.True(_petsManagementService.UpdatePet(pet));
        }


    }
}