using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;

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