using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Core.Services
{
    public class RestService : IRestService
    {
        private const string RestMediaType = "application/json";

        public async Task<bool> ExecuteDeleteAction<T>(string address, T obj)
        {
            if(obj == null)
            {
                return false;
            }


            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.DeleteAsync(address);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }

        public async Task<T> ExecuteGetAction<T>(string address)
        {            
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(address);
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    T output = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringResponse);
                    return output;
                }
                catch(HttpRequestException)
                {
                    return default(T);
                }
            }
        }

        public async Task<bool> ExecutePostAction<T>(string address, T obj)
        {
            if(obj == null)
            {
                return false;
            }

            var serializedObj = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(serializedObj, Encoding.UTF8, RestMediaType);

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(address, httpContent);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch(HttpRequestException)
                {
                    return false;
                }
            }
        }
        public async Task<bool> ExecutePutAction<T>(string address, T obj) 
        {
            if(string.IsNullOrEmpty(address) || obj == null)
            {
                return false;
            }
            
            var serializedObject = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(serializedObject, Encoding.UTF8, RestMediaType);
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PutAsync(address, httpContent);
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