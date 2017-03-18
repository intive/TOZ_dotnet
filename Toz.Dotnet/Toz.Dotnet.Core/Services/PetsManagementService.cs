using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;
using System.Text;

namespace Toz.Dotnet.Core.Services
{
    public class PetsManagementService : IPetsManagementService
    {
        private IFilesManagementService _filesManagementService;
        private readonly AppSettings _appSettings;
        private List<Pet> _mockupPetsDatabase;



        public PetsManagementService(IFilesManagementService filesManagementService, IOptions<AppSettings> appSettings)
        {
            _filesManagementService = filesManagementService;
            _mockupPetsDatabase = new List<Pet>();
            _appSettings = appSettings.Value;
        }

		public async Task<List<Pet>> GetAllPets()
        {
            string address = _appSettings.BackendPetsUrl;
            
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
                    return new List<Pet>();
                }
            }
        }
		

        public async Task<bool> UpdatePet(Pet pet)
        {
           string address = $"{_appSettings.BackendPetsUrl}/{pet.Id}";
           var serializedPet = Newtonsoft.Json.JsonConvert.SerializeObject(pet);
           var httpContent = new StringContent(serializedPet, Encoding.UTF8, "application/json");
            
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PutAsync(address, httpContent);
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    
                    return true;
                }
                catch(HttpRequestException ex)
                {
                    return false;
                }
            }
        }

        
        public async Task<bool> CreatePet(Pet pet)
        {
            if(pet == null)
            {
                return false;
            }

            var address = _appSettings.BackendPetsUrl;
            var serializedPet = Newtonsoft.Json.JsonConvert.SerializeObject(pet);
            var httpContent = new StringContent(serializedPet, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(address, httpContent);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch(HttpRequestException ex)
                {
                    return false;
                }
            }
        }

        public bool DeletePet(Pet pet)
        {
            return true;
        }

        public async Task<Pet> GetPet(string id)
        {
            string address = $"{_appSettings.BackendPetsUrl}/{id}";
            
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(address);
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    Pet output = Newtonsoft.Json.JsonConvert.DeserializeObject<Pet>(stringResponse);
                    return output;
                }
                catch(HttpRequestException ex)
                {
                    return new Pet();
                }
            }
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