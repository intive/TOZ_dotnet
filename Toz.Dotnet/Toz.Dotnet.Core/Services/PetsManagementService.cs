using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;
using System.Threading;

namespace Toz.Dotnet.Core.Services
{
    public class PetsManagementService : IPetsManagementService
    {
        private IRestService _restService;
        private IFilesManagementService _filesManagementService;
        private List<Pet> _mockupPetsDatabase;
        public string RequestUri { get; set; }

        public PetsManagementService(IFilesManagementService filesManagementService, IRestService restService, IOptions<AppSettings> appSettings)
        {
            _filesManagementService = filesManagementService;
            _mockupPetsDatabase = new List<Pet>();
            _restService = restService;

            RequestUri = appSettings.Value.BackendPetsUrl;
        }

		public async Task<List<Pet>> GetAllPets(CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = RequestUri;
            
            return await _restService.ExecuteGetAction<List<Pet>>(address, cancelationToken);
        }
		
        
        public async Task<bool> UpdatePet(Pet pet, CancellationToken cancelationToken = default(CancellationToken))
        {
           var address = $"{RequestUri}/{pet.Id}";
           return await _restService.ExecutePutAction(address, pet, cancelationToken);
        }

        
        public async Task<bool> CreatePet(Pet pet, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, pet, cancelationToken);
        }

        public async Task<bool> DeletePet(Pet pet, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{pet.Id}";
            return await _restService.ExecuteDeleteAction(address, pet, cancelationToken);
        }

        public async Task<Pet> GetPet(string id, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Pet>(address, cancelationToken);
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