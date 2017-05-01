using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading;

namespace Toz.Dotnet.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private JwtToken _token;

        public bool IsAuth { get; private set; }
        public string ActiveUser { get; private set; }
        public string RequestUri { get; set; }

        public AuthService(IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
            IsAuth = false;
            RequestUri = _appSettings.BackendJwtUrl;
        }

        public async Task SignIn(Login login)
        {

            string serializedObject = JsonConvert.SerializeObject(login, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var httpContent = new StringContent(serializedObject, System.Text.Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(RequestUri, httpContent);
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();
                    _token = Newtonsoft.Json.JsonConvert.DeserializeObject<JwtToken>(responseString);
                    IsAuth = true;
                    ActiveUser = login.Email;
                }
                catch(HttpRequestException)
                {
                    IsAuth = false;
                }
            }
        }

        public void SighOut()
        {
            _token = null;
            IsAuth = false;
        }

        public void AddTokenToHttpClient(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token.jwt}");
        }
    }
}