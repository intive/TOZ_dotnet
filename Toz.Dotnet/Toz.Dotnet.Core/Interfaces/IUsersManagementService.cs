using System.Collections.Generic;
using System.Threading.Tasks;
using Toz.Dotnet.Models;
using System.Threading;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IUsersManagementService
    {
		Task<User> GetUser(string id, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<User>> GetAllUsers(CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdateUser(User user, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteUser(User user, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateUser(User user, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<User>> FindAllUsers(string firstName, string lastName, CancellationToken cancelationToken = default(CancellationToken));
        Task<User> FindUser(string firstName, string lastName, CancellationToken cancelationToken = default(CancellationToken));
        void SetupSampleUsers(CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}