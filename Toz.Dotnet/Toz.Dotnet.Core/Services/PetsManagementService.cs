using System;
using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

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
            return true;
        }

        
        public bool CreatePet(Pet pet)
        {
            return true;
        }

        public bool DeletePet(Pet pet)
        {
            return true;
        }

        public Pet GetPet(int id)
        {
            return null;
        }

        public byte[] ConvertPhotoToByteArray(Stream fileStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}