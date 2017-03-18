using System;
using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Toz.Dotnet.Models.JsonConventers;

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

		public async Task<List<Pet>> GetAllPets()
        {
            string address = "http://dev.patronage2017.intive-projects.com/pets";
            
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(address);
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    List<Pet> output = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pet>>(stringResponse);
                    return output;
                }
                catch(HttpRequestException ex)
                {
                    throw;
                }
            }
            

        }
		

        public bool UpdatePet(Pet pet)
        {
            if(pet != null)
            {
                pet.LastEditTime = DateTime.Now;

                return true;
            }
            return false;
        }

        
        public bool CreatePet(Pet pet)
        {
            int? newId;

            if(pet != null && (newId = GetFirstAvailableId()) != null )
            {
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
                return null; 
            }
            return null;
        }

        private int? GetFirstAvailableId()
        {           


            return 1;
        }
    }
}