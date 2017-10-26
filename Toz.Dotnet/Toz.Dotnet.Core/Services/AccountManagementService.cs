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
        public string RequestUriJwt { get; set; }
        public string RequestUriForgetPassword { get; set; }

        public AccountManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUriJwt = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendJwtUrl;
            RequestUriForgetPassword = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendForgotPasswordUrl;
        }

        public async Task<JwtToken> SignIn(Login login, CancellationToken cancelationToken = default(CancellationToken))
        {
            return await _restService.ExecutePostAction<JwtToken, Login>(RequestUriJwt, login, cancelationToken: cancelationToken);
        }

        public async Task<bool> ForgotPassword(ForgotPassword forgotPassword, CancellationToken cancelationToken = default(CancellationToken))
        {
            return await _restService.ExecutePostAction<ForgotPassword>(RequestUriForgetPassword, forgotPassword, cancelationToken: cancelationToken);
        }
    }
}
