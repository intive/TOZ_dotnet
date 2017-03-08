using System;
using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Core.Services
{
    public class PetsManagementService : IPetsManagementService
    {
        private IFilesManagementService _filesManagementService;
        private List<Pet> _mockupPetsDatabase;

        public PetsManagementService(IFilesManagementService filesManagementService)
        {
            _filesManagementService = filesManagementService;
            _mockupPetsDatabase = new List<Pet>();
        }

        public List<Pet> GetSamplePetsList()
        {
            List<Pet> petsList = new List<Pet>(){
                new Pet(0,"Tofik",PetType.Dog,PetSex.Female,new byte[0],"Przyjacielski piesek","Znaleziono na Bohaterow Warszawy",DateTime.Parse("2017-03-06 13:26"),DateTime.Parse("2017-03-07 13:55")),
                new Pet(1,"Filemon",PetType.Cat,PetSex.Male,new byte[0],"Przyjacielski kotek","Znaleziono na Wydziale Informatyki ZUT",DateTime.Parse("2017-03-05 22:11"),DateTime.Parse("2017-03-07 12:55")),
                new Pet(2,"Bonifacy",PetType.Cat,PetSex.Male,new byte[0],"Bystry kotek","Znaleziony w Parku Kasprowicza",DateTime.Parse("2017-03-07 10:11"),DateTime.Parse("2017-03-07 12:34"))
            };
            
            return petsList;
        }

        public void AddPet(Pet pet)
        {
            _mockupPetsDatabase.Add(pet);
        }

        public List<Pet> GetPetsList()
        {
            return _mockupPetsDatabase;
        }
    }
}