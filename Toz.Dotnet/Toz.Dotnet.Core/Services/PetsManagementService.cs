using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Models;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class PetsManagementService : IPetsManagementService
    {
        private IRestService _restService;
        private IFilesManagementService _filesManagementService;
        public string RequestUri { get; set; }

        public PetsManagementService(IFilesManagementService filesManagementService, IRestService restService, IOptions<AppSettings> appSettings)
        {
            _filesManagementService = filesManagementService;
            _restService = restService;

            RequestUri = appSettings.Value.BackendPetsUrl;
        }

		public async Task<List<Pet>> GetAllPets(string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = RequestUri;
            return await _restService.ExecuteGetAction<List<Pet>>(address, token, cancelationToken);
        }
		
        
        public async Task<bool> UpdatePet(Pet pet, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
           var address = $"{RequestUri}/{pet.Id}";
           return await _restService.ExecutePutAction(address, pet, token, cancelationToken);
        }

        
        public async Task<bool> CreatePet(Pet pet, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, pet, token, cancelationToken);
        }

        public async Task<bool> DeletePet(Pet pet, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{pet.Id}";
            return await _restService.ExecuteDeleteAction(address, token, cancelationToken);
        }

        public async Task<Pet> GetPet(string id, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Pet>(address, token, cancelationToken);
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