using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Errors;

namespace Toz.Dotnet.Core.Services
{
    public class RestService : IRestService
    {
        private const string RestMediaType = "application/json";
        private IBackendErrorsService _backendErrorsService;

        public RestService(IBackendErrorsService backendErrorsService)
        {
            _backendErrorsService = backendErrorsService;
        }

        public async Task<bool> ExecuteDeleteAction(string address, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            using (var client = CreateHttpClient(token))
            {
                try
                {
                    var response = await client.DeleteAsync(address, cancelationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        await PassJsonResponseToErrorService(response);
                        return false;
                    }
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }

        public async Task<T> ExecuteGetAction<T>(string address, string token, CancellationToken cancelationToken = default(CancellationToken)) where T : class
{            
            using (var client = CreateHttpClient(token))
            {
                try
                {
                    var response = await client.GetAsync(address, cancelationToken);
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                   

                    T output = JsonConvert.DeserializeObject<T>(stringResponse);
                    return output;
                }
                catch(HttpRequestException)
                {
                    return default(T);
                }
            }
        }

        public async Task<bool> ExecutePostAction<T>(string address, T obj, string token, CancellationToken cancelationToken = default(CancellationToken)) where T : class
        {
            if(obj == null)
            {
                return false;
            }

            string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            var httpContent = new StringContent(serializedObject, Encoding.UTF8, RestMediaType);

            using (var client = CreateHttpClient(token))
            {
                try
                {
                    var response = await client.PostAsync(address, httpContent, cancelationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        await PassJsonResponseToErrorService(response);
                        return false;
                    }
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }

        public async Task<T1> ExecutePostAction<T1, T2>(string address, T2 obj, string token = default(string), CancellationToken cancelationToken = default(CancellationToken)) where T1: class where T2: class
        {
            if (obj == null)
            {
                return null;
            }

            string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var httpContent = new StringContent(serializedObject, Encoding.UTF8, RestMediaType);

            using (var client = CreateHttpClient(token))
            {
                try
                {
                    var response = await client.PostAsync(address, httpContent, cancelationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        await PassJsonResponseToErrorService(response);
                        return null;
                    }
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T1>(responseString);
                }
                catch (HttpRequestException)
                {
                    return null;
                }
            }
        }

        public async Task<bool> ExecutePutAction<T>(string address, T obj, string token, CancellationToken cancelationToken = default(CancellationToken)) where T : class
{
            if(string.IsNullOrEmpty(address) || obj == null)
            {
                return false;
            }
            
            string serializedObject = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });

            var httpContent = new StringContent(serializedObject, Encoding.UTF8, RestMediaType);
            using (var client = CreateHttpClient(token))
            {
                try
                {
                    var response = await client.PutAsync(address, httpContent, cancelationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        await PassJsonResponseToErrorService(response);
                        return false;
                    }
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }

        private async Task PassJsonResponseToErrorService(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            ErrorsList output = JsonConvert.DeserializeObject<ErrorsList>(stringResponse);
            if (output.Errors == null)
            {
                var error = JsonConvert.DeserializeObject<Error>(stringResponse);
                if (error != null)
                {
                    output.Errors = new List<Error>() { error };
                }
            }
            _backendErrorsService.AddErrors(output);
        }

        private HttpClient CreateHttpClient(string token = default(string))
        {
            var client = new HttpClient();
            if (token != default(string))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
            return client;
        }
    }
}