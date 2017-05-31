using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Models;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;
using System.Linq;

namespace Toz.Dotnet.Core.Services
{
    public class PetsStatusManagementService : IPetsStatusManagementService
    {
        private IRestService _restService;
        public string RequestUri { get; set; }

        public PetsStatusManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendPetsUrl + appSettings.Value.BackendPetsStatusUrl;
        }

		public async Task<List<PetsStatus>> GetAllStatus(string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = RequestUri;
            return await _restService.ExecuteGetAction<List<PetsStatus>>(address, token, cancelationToken);
        }
		
        
        public async Task<bool> UpdateStatus(PetsStatus status, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
           var address = $"{RequestUri}/{status.Id}";
           return await _restService.ExecutePutAction(address, status, token, cancelationToken);
        }

        
        public async Task<bool> CreateStatus(PetsStatus status, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, status, token, cancelationToken);
        }

        public async Task<bool> DeleteStatus(PetsStatus status, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{status.Id}";
            return await _restService.ExecuteDeleteAction(address, token, cancelationToken);
        }

        public async Task<PetsStatus> GetStatus(string id, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            //:disappointed:
            List<PetsStatus> statusList = await GetAllStatus(token, cancelationToken);
            return statusList.Where(x => x.Id == id).First();
        }
    }
}