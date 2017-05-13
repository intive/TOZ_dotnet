using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Models;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Linq;

namespace Toz.Dotnet.Core.Services
{
    //TODO: uncomment everything when backend is ready
    public class UsersManagementService : IUsersManagementService
    {
        private const int DefaultUserIndex = 0;

        //private IRestService _restService;
        private List<User> _mockupUsersDatabase;
        private Random r = new Random();
        
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

        public User GetRandomVolunteer(CancellationToken cancelationToken = default(CancellationToken))
        {
            var volunteers = _mockupUsersDatabase.Where(p => p.Purpose == UserType.Volunteer).ToList();
            int count = volunteers.Count();
            return volunteers[r.Next(0, count-1)];
            //string address = $"{RequestUri}/{id}";
            //return await _restService.ExecuteGetAction<User>(address, cancelationToken);
        }

        //Returns list of users with typed firstName and lastName or empty list if not found.
        public async Task<List<User>> FindAllUsers(string firstName, string lastName, CancellationToken cancelationToken = default(CancellationToken))
        {
            return _mockupUsersDatabase.FindAll(u => u.FirstName.Equals(firstName) && u.LastName.Equals(lastName));
        }

        //Returns first occurance of user with typed firstName and lastName or null if not found
        public async Task<User> FindUser(string firstName, string lastName, CancellationToken cancelationToken = default(CancellationToken))
        {
            List<User> users = await FindAllUsers(firstName, lastName, cancelationToken);

            if (users.Count > 0)
            {
                return users[DefaultUserIndex];
            }
            else
            {
                return null;
            }
        }
        
        public async void SetupSampleUsers(CancellationToken cancelationToken = default(CancellationToken))
        {
            _mockupUsersDatabase.Clear();

            await CreateUser(new User
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                PhoneNumber = "012345678",
                Email = "jan.kowalski@onet.eu",
                Purpose = UserType.Administrator
            });

            await CreateUser(new User
            {
                FirstName = "Adrian",
                LastName = "Standowicz",
                PhoneNumber = "123456789",
                Email = "adrian.standowicz@gmail.com",
                Purpose = UserType.Volunteer
            });

            await CreateUser(new User
            {
                FirstName = "Daniel",
                LastName = "Kleszczyński",
                PhoneNumber = "234567890",
                Email = "danielk@o2.pl",
                Purpose = UserType.Volunteer
            });

            await CreateUser(new User
            {
                FirstName = "Łukasz",
                LastName = "Sielewicz",
                PhoneNumber = "345678901",
                Email = "lsielewicz@hotmail.eu",
                Purpose = UserType.Volunteer
            });

            await CreateUser(new User
            {
                FirstName = "Krystian",
                LastName = "Kaniewski",
                PhoneNumber = "456789012",
                Email = "kania@wp.pl",
                Purpose = UserType.Volunteer
            });

            await CreateUser(new User
            {
                FirstName = "Bartosz",
                LastName = "Hliwa",
                PhoneNumber = "567890123",
                Email = "hliwart@op.pl",
                Purpose = UserType.Volunteer
            });

            await CreateUser(new User
            {
                FirstName = "Mateusz",
                LastName = "Kumpf",
                PhoneNumber = "678901234",
                Email = "matkumpf@gmail.com",
                Purpose = UserType.Volunteer
            });

            await CreateUser(new User
            {
                FirstName = "Huber",
                LastName = "Taler",
                PhoneNumber = "789012345",
                Email = "h.taler@intive.com",
                Purpose = UserType.TemporaryHome
            });

            await CreateUser(new User
            {
                FirstName = "Sebastian",
                LastName = "Peć",
                PhoneNumber = "890123456",
                Email = "spec@intive.com",
                Purpose = UserType.TemporaryHome
            });
        }
    }
}