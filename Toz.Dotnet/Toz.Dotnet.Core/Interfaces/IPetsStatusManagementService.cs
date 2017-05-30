using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Toz.Dotnet.Models;
using System.Threading;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IPetsStatusManagementService
    {
		Task<PetsStatus> GetStatus(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<PetsStatus>> GetAllStatus(string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdateStatus(PetsStatus status, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteStatus(PetsStatus status, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateStatus(PetsStatus status, string token, CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}