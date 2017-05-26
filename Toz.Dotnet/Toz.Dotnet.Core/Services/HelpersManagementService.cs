using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class HelpersManagementService : IHelpersManagementService
    {
        private readonly IRestService _restService;
        public string RequestUri { get; set; }
        
        public HelpersManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendHelpersUrl;
        }

        public async Task<Helper> GetHelper(string id, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Helper>(address, token, cancelationToken);
        }

        public async Task<List<Helper>> GetAllHelpers(string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = RequestUri;
            return await _restService.ExecuteGetAction<List<Helper>>(address, token, cancelationToken);
        }

        public async Task<bool> CreateHelper(Helper helper, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, helper, token, cancelationToken);
        }
        
        public async Task<bool> UpdateHelper(Helper helper, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{helper.Id}";
            return await _restService.ExecutePutAction(address, helper, token, cancelationToken);
        }
        
        public async Task<bool> DeleteHelper(string id, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{id}";
            return await _restService.ExecuteDeleteAction(address, token, cancelationToken);
        }
    }
}
