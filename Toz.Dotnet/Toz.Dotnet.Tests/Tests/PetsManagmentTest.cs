using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public void TestDependencyInjectionFromPetsManagementService()
        {
            Assert.NotNull(_petsManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_petsManagementService.RequestUri));
        }

        [Fact]
        public void TestOfGettingAllPets()
        {
            Assert.NotNull(_petsManagementService.GetAllPets().Result);
        }
         
        [Fact]
        public void TestOfCreatingNewPet()
        {
            Assert.True(_petsManagementService.CreatePet(_testingPet).Result);
            _petsManagementService.DeletePet(_testingPet).Wait();
        }

        [Fact]
        public void TestOfDeletingSpecifiedPet()
        {
            var pets = _petsManagementService.GetAllPets().Result;
            if(pets.Any())
            {
                var firstPet = pets.FirstOrDefault();
                Assert.True(_petsManagementService.DeletePet(firstPet).Result);
                Assert.True(_petsManagementService.CreatePet(firstPet).Result);
            }
        }

        [Fact]
        public void TestOfGettingSpecifiedPet()
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
        public void TestOfPetUpdating()
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
        
        [Fact]
        public void TestPetValidationIfCorrectData()
        {
            // Arrange
            var context = new ValidationContext(_testingPet, null, null);
            var result = new List<ValidationResult>();

            // Act
            bool valid = Validator.TryValidateObject(_testingPet, context, result, true);

            Assert.True(valid);
        }

        [Theory]
        [InlineData("Type")]
        public void TestPetValidationIfRequiredPropertyIsNotInitialized(string property)
        {
            // Arrange
            Pet pet = ClonePet(_testingPet);

            if (property.Equals("Type"))
            {
                pet.Type = PetType.UNIDENTIFIED;
            }
           
            var context = new ValidationContext(pet, null, null);
            var result = new List<ValidationResult>();

            // Act
            bool valid = Validator.TryValidateObject(pet, context, result, true);

            Assert.False(valid);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Address")]
        [InlineData("Description")]
        public void TestPetValidationIfStringIsTooLong(string property)
        {
            // Arrange
            Pet pet = ClonePet(_testingPet);

            if (property.Equals("Name")) 
            { 
                pet.Name = new string('x', 40);
            }
            else if (property.Equals("Address"))
            {
                pet.Address = new string('x', 101);
            }
            else if (property.Equals("Description"))
            {
                pet.Description = new string('x', 310);
            }

            var context = new ValidationContext(pet, null, null);
            var result = new List<ValidationResult>();

            //Act
            bool valid = Validator.TryValidateObject(pet, context, result, true);

            Assert.False(valid);                      
        }

/*
        [Theory]
        [InlineData("CR7")]
        [InlineData("     ")]
        public void TestPetValidationIfRegexNotMatch(string value)
        {
            //Arrange
            Pet pet = ClonePet(_testingPet);
            pet.Name = value;

            var context = new ValidationContext(pet, null, null);
            var result = new List<ValidationResult>();

            //Act
            bool valid = Validator.TryValidateObject(pet, context, result, true);

            Assert.False(valid);
        }*/

        private Pet ClonePet(Pet pet)
        {
            return new Pet()
            {
                Id = pet.Id,
                Name = pet.Name,
                Type = pet.Type,
                Sex = pet.Sex,
                Photo = (byte[])pet.Photo.Clone(),
                Description = pet.Description,
                Address = pet.Address,
                AddingTime = pet.AddingTime,
                LastEditTime = pet.LastEditTime
            };
        }
        
    }
}