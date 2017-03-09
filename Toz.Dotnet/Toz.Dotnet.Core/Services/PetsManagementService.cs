using System;
using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System.Linq;

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

        
        public bool CreatePet(Pet pet)
        {
            int? newId;

            if(pet != null && (newId = GetFirstAvailableId()) != null )
            {
                pet.Id = (int)newId;
                pet.AddingTime = DateTime.Now;
                pet.LastEditTime = DateTime.Now;          
                _mockupPetsDatabase.Add(pet);
                return true;
            }
            return false;
        }

        public bool DeletePet(Pet pet)
        {
            if(pet != null && _mockupPetsDatabase.Contains(pet))
            {
                _mockupPetsDatabase.Remove(pet);
                return true;
            }
            return false;
        }

        public Pet GetPet(int id)
        {
            if(id >= 0)
            {
                return _mockupPetsDatabase[id]; 
            }
            return null;
        }

        private int? GetFirstAvailableId()
        {           
            IEnumerable<int> takenIds = _mockupPetsDatabase.Select(p => p.Id).ToList();
            int? availableId;

            try
            {
                availableId = Enumerable.Range(0, int.MaxValue).Except(takenIds).First();
            }
            catch (InvalidOperationException)
            {
                availableId = null;
            }

            return availableId;
        }
    }
}