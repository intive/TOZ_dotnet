using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Xunit.Extensions;

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
        [InlineData("Name")]
        [InlineData("Address")]
        [InlineData("Type")]
        public void TestPetValidationIfRequiredPropertyIsNotInitialized(string property)
        {
            // Arrange
            Pet pet = ClonePet(_testingPet);

            if (property.Equals("Name")) 
            { 
                pet.Name = "";
            }
            else if (property.Equals("Address"))
            {
                pet.Address = "";
            }
            else if (property.Equals("Type"))
            {
                pet.Type = PetType.Unidentified;
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
        }

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