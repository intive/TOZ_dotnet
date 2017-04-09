using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Models;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    //TODO: uncomment everything when backend is ready
    public class UsersManagementService : IUsersManagementService
    {     
        //private IRestService _restService;
        private List<User> _mockupUsersDatabase;

        public string RequestUri { get; set; }

        public UsersManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _mockupUsersDatabase = new List<User>();
            //_restService = restService;
            //RequestUri = appSettings.Value.BackendUsersUrl;
        }

		public async Task<List<User>> GetAllUsers(CancellationToken cancelationToken = default(CancellationToken))
        {
            return _mockupUsersDatabase;
            //string address = RequestUri;
            //return await _restService.ExecuteGetAction<List<User>>(address, cancelationToken);
        }
		     
        public async Task<bool> UpdateUser(User user, CancellationToken cancelationToken = default(CancellationToken))
        {
            User userFromDb = await GetUser(user.Id);
            if(userFromDb != null)
            {
                userFromDb = user;
                return true;
            }
            return false;
           //var address = $"{RequestUri}/{user.Id}";
           //return await _restService.ExecutePutAction(address, user, cancelationToken);
        }
        
        public async Task<bool> CreateUser(User user, CancellationToken cancelationToken = default(CancellationToken))
        {
            if(!_mockupUsersDatabase.Exists(u => u.Id == user.Id))
            {
                _mockupUsersDatabase.Add(user);
                user.Id = _mockupUsersDatabase.IndexOf(user).ToString();
                return true;
            }
            return false;
            //var address = RequestUri;
            //return await _restService.ExecutePostAction(address, user, cancelationToken);
        }

        public async Task<bool> DeleteUser(User user, CancellationToken cancelationToken = default(CancellationToken))
        {
            User userFromDb = await GetUser(user.Id);
            if(userFromDb != null)
            {
                _mockupUsersDatabase.Remove(userFromDb);
                return true;
            }
            return false;
            //var address = $"{RequestUri}/{user.Id}";
            //return await _restService.ExecuteDeleteAction(address, user, cancelationToken);
        }

        public async Task<User> GetUser(string id, CancellationToken cancelationToken = default(CancellationToken))
        {
            return _mockupUsersDatabase.Find(u => u.Id == id);
            //string address = $"{RequestUri}/{id}";
            //return await _restService.ExecuteGetAction<User>(address, cancelationToken);
        }

        public async Task<User> FindUser(string firstName, string lastName, CancellationToken cancelationToken = default(CancellationToken))
        {
            //Return user with typed firstName and lastName or null if not found.
            return _mockupUsersDatabase.Find(u => u.FirstName.Equals(firstName) && u.LastName.Equals(lastName));
        }
    }
}