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
        private IRestService _restService;
        private IFilesManagementService _filesManagementService;
        private readonly AppSettings _appSettings;
        private List<Pet> _mockupPetsDatabase;

        public PetsManagementService(IFilesManagementService filesManagementService, IRestService restService, IOptions<AppSettings> appSettings)
        {
            _filesManagementService = filesManagementService;
            _mockupPetsDatabase = new List<Pet>();
            _appSettings = appSettings.Value;
            _restService = restService;
        }

		public async Task<List<Pet>> GetAllPets()
        {
            string address = _appSettings.BackendPetsUrl;
            
            return await _restService.ExecuteGetAction<List<Pet>>(address);
        }
		
        
        public async Task<bool> UpdatePet(Pet pet)
        {
           var address = $"{_appSettings.BackendPetsUrl}/{pet.Id}";
           return await _restService.ExecutePutAction(address, pet);
        }

        
        public async Task<bool> CreatePet(Pet pet)
        {
            var address = _appSettings.BackendPetsUrl;
            return await _restService.ExecutePostAction(address, pet);
        }

        public async Task<bool> DeletePet(Pet pet)
        {
            var address = $"{_appSettings.BackendPetsUrl}/{pet.Id}";
            return await _restService.ExecuteDeleteAction(address, pet);
        }

        public async Task<Pet> GetPet(string id)
        {
            string address = $"{_appSettings.BackendPetsUrl}/{id}";
            return await _restService.ExecuteGetAction<Pet>(address);
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