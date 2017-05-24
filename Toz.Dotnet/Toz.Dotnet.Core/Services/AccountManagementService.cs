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
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IRestService _restService;
        public string RequestUri { get; set; }

        public AccountManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendJwtUrl;
        }

        public async Task<JwtToken> SignIn(Login login, CancellationToken cancelationToken = default(CancellationToken))
        {
            return await _restService.ExecutePostAction<JwtToken, Login>(RequestUri, login, cancelationToken: cancelationToken);
        }
    }
}
