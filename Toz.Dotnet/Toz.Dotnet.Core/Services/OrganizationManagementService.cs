using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class OrganizationManagementService : IOrganizationManagementService
    {
        private readonly IRestService _restService;
        public string RequestUri { get; set; }

        public OrganizationManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendOrganizationInfoUrl;
        }

        public async Task<bool> UpdateOrCreateInfo(Organization organizationInfo, CancellationToken cancelationToken = default(CancellationToken))
        {
            Func<string, Organization, CancellationToken, Task<bool>> methodToExecute = _restService.ExecutePutAction;

            if (await GetOrganizationInfo(cancelationToken) == null)
            {
                methodToExecute = _restService.ExecutePostAction;
            }

            return await methodToExecute(RequestUri, organizationInfo, cancelationToken);
        }

        public async Task<Organization> GetOrganizationInfo(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _restService.ExecuteGetAction<Organization>(RequestUri, cancellationToken);
        }
    }
}
