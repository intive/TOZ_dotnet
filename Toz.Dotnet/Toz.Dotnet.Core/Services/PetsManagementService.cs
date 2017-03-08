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

		public List<Pet> GetAllPets()
        {
            return _mockupPetsDatabase;
        }
		
        public List<Pet> GetSamplePets()
        {
            List<Pet> petsList = new List<Pet>(){
                new Pet(0,"Tofik",PetType.Dog,PetSex.Female,new byte[0],"Przyjacielski piesek","Znaleziono na Bohaterow Warszawy",DateTime.Parse("2017-03-06 13:26"),DateTime.Parse("2017-03-07 13:55")),
                new Pet(1,"Filemon",PetType.Cat,PetSex.Male,new byte[0],"Przyjacielski kotek","Znaleziono na Wydziale Informatyki ZUT",DateTime.Parse("2017-03-05 22:11"),DateTime.Parse("2017-03-07 12:55")),
                new Pet(2,"Bonifacy",PetType.Cat,PetSex.Male,new byte[0],"Bystry kotek","Znaleziony w Parku Kasprowicza",DateTime.Parse("2017-03-07 10:11"),DateTime.Parse("2017-03-07 12:34"))
            };
            
            return petsList;
        }

        public bool UpdatePet(Pet pet)
        {
            if(pet != null)
            {
                //todo add all the backend magic to update our pet
                return true;
            }
            return false;
        }

        
        public bool AddPet(Pet pet)
        {
            if(pet != null)
            {
                _mockupPetsDatabase.Add(pet);
                return true;
            }
            return false;
        }

        public bool DeletePet(Pet pet)
        {
            if(pet != null && _mockupPetsDatabase.contains(pet))
            {
                _mockupPetsDatabase.remove(pet);
                return true;
            }
            return false;
        }

        public Pet GetPet(int id)
        {
            if(id >= 0)
            {
                //todo replace example pet with real functionality that asks backend
                var pet = new Pet()
                {
                    Id = 123,
                    Name = "TestDog",
                    Type = PetType.Dog,
                    Sex = PetSex.Male,
                    Photo = new byte[10],
                    Description = "Dog that eats tigers",
                    Address = "Found in jungle",
                    AddingTime = DateTime.Now,
                    LastEditTime = DateTime.Now
                };
                return pet; 
            }
            return null;
        }
    }
}