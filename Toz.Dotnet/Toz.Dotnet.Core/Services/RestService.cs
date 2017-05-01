using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Core.Services
{
    public class RestService : IRestService
    {
        private const string RestMediaType = "application/json";
        private readonly IAuthService _authService; // TEMPORARY

        public RestService(IAuthService authService)
        {
            _authService = authService; // TEMPORARY
        }

        public async Task<bool> ExecuteDeleteAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken))
        {
            if(obj == null)
            {
                return false;
            }

            using (var client = new HttpClient())
            {
                try
                {
                    // --> TEMPORARY
                    if (_authService.IsAuth)
                    {
                        _authService.AddTokenToHttpClient(client);
                    }
                    // <--
                    var response = await client.DeleteAsync(address, cancelationToken);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }

        public async Task<T> ExecuteGetAction<T>(string address, CancellationToken cancelationToken = default(CancellationToken))
        {            
            using (var client = new HttpClient())
            {
                try
                {
                    // --> TEMPORARY
                    if (_authService.IsAuth)
                    {
                        _authService.AddTokenToHttpClient(client);
                    }
                    // <--
                    var response = await client.GetAsync(address, cancelationToken);
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    T output = JsonConvert.DeserializeObject<T>(stringResponse);
                    return output;
                }
                catch(HttpRequestException)
                {
                    return default(T);
                }
            }
        }

        public async Task<bool> ExecutePostAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken))
        {
            if(obj == null)
            {
                return false;
            }

            string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            var httpContent = new StringContent(serializedObject, Encoding.UTF8, RestMediaType);

            using (var client = new HttpClient())
            {
                try
                {
                    // --> TEMPORARY
                    if (_authService.IsAuth)
                    {
                        _authService.AddTokenToHttpClient(client);
                    }
                    // <--
                    var response = await client.PostAsync(address, httpContent, cancelationToken);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }

        public async Task<bool> ExecutePutAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken)) 
        {
            if(string.IsNullOrEmpty(address) || obj == null)
            {
                return false;
            }
            
            string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });

            var httpContent = new StringContent(serializedObject, Encoding.UTF8, RestMediaType);
            using (var client = new HttpClient())
            {
                try
                {
                    // --> TEMPORARY
                    if (_authService.IsAuth)
                    {
                        _authService.AddTokenToHttpClient(client);
                    }
                    // <--
                    var response = await client.PutAsync(address, httpContent, cancelationToken);
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }

    }
}