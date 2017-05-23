using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Toz.Dotnet.Tests.Tests
{
    public class PetsManagementTest
    {
        private readonly Pet _testingPet;
        private readonly IPetsManagementService _petsManagementService;
        private readonly JwtToken _token;

        public PetsManagementTest()
        { 
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
            _testingPet = TestingObjectProvider.Instance.Pet;
            _token = TestingObjectProvider.Instance.JwtToken;
        }

        [Fact]
        public void TestDependencyInjection()
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
            Assert.NotNull(_petsManagementService.GetAllPets(_token.Jwt).Result);
        }
         
        [Fact]
        public void TestOfCreatingNewPet()
        {
            var result = _petsManagementService.CreatePet(_testingPet, _token.Jwt).Result;
            Assert.True(result);
        }
        
        [Fact]
        public void TestOfDeletingSpecifiedPet()
        {
            Assert.True(_petsManagementService.DeletePet(_testingPet, _token.Jwt).Result);
        }
        
        [Fact]
        public void TestOfGettingSpecifiedPet()
        {
            Assert.NotNull(_petsManagementService.GetPet(_testingPet.Id, _token.Jwt).Result);        
        }

        [Fact]
        public void TestOfUpdatingPet()
        {
            Assert.True(_petsManagementService.UpdatePet(_testingPet, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfUpdatingPetWithNullValue()
        {
            var exception = Record.Exception(() => _petsManagementService.UpdatePet(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfDeletingPetThatIsNull()
        {
            var exception = Record.Exception(() => _petsManagementService.DeletePet(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllPetsUsingWrongUrl()
        {
            _petsManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_petsManagementService.GetAllPets(_token.Jwt).Result);
            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
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
            //Pet pet = ClonePet(_testingPet);
            var pet = TestingObjectProvider.Instance.DoShallowCopy<Pet>(_testingPet);

            if (property.Equals("Type"))
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
            var pet = TestingObjectProvider.Instance.DoShallowCopy(_testingPet);

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

        [Fact]
        public void TestPetValidationIfPetTypeIsUndentified()
        {
            var pet = TestingObjectProvider.Instance.DoShallowCopy(_testingPet);
            pet.Type = PetType.Unidentified;
            var validationContext = new ValidationContext(pet, null, null);
            var validationResult = new List<ValidationResult>();

            Assert.False(Validator.TryValidateObject(pet, validationContext, validationResult, true));
        }

        [Fact]
        public void TestPetValidationIfPetSexIsUnknown()
        {
            var pet = TestingObjectProvider.Instance.DoShallowCopy(_testingPet);
            pet.Sex = PetSex.Unknown;
            var validationContext = new ValidationContext(pet, null, null);
            var validationResult = new List<ValidationResult>();

            Assert.False(Validator.TryValidateObject(pet, validationContext, validationResult, true));
        }

        [Fact]
        public void TestPetPhotoConvertingToByteArray()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("Test"));
            var streamBytes = _petsManagementService.ConvertPhotoToByteArray(stream);
            Assert.NotNull(streamBytes);
            Assert.True(streamBytes.Length > 0);
        }

    }
}
