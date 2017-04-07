using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Models;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class UsersManagementService : IUsersManagementService
    {     
        private IRestService _restService;
        public string RequestUri { get; set; }

        public UsersManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendUsersUrl;
        }

		public async Task<List<User>> GetAllUsers(CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = RequestUri;
            return await _restService.ExecuteGetAction<List<User>>(address, cancelationToken);
        }
		     
        public async Task<bool> UpdateUser(User user, CancellationToken cancelationToken = default(CancellationToken))
        {
           var address = $"{RequestUri}/{user.Id}";
           return await _restService.ExecutePutAction(address, user, cancelationToken);
        }
        
        public async Task<bool> CreateUser(User user, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, user, cancelationToken);
        }

        public async Task<bool> DeleteUser(User user, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{user.Id}";
            return await _restService.ExecuteDeleteAction(address, user, cancelationToken);
        }

        public async Task<User> GetUser(string id, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<User>(address, cancelationToken);
        }
    }
}