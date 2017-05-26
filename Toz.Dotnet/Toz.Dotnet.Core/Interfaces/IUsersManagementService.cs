using System.Collections.Generic;
using System.Threading.Tasks;
using Toz.Dotnet.Models;
using System.Threading;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IUsersManagementService
    {
		Task<User> GetUser(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<User>> GetAllUsers(string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdateUser(User user, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteUser(User user, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateUser(User user, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<User>> FindAllUsers(string firstName, string lastName, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<User> FindUser(string firstName, string lastName, string token, CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}