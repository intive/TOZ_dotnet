using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IHelpersManagementService
    {
        Task<Helper> GetHelper(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<Helper>> GetAllHelpers(string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdateHelper(Helper helper, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteHelper(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateHelper(Helper helper, string token, CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}
