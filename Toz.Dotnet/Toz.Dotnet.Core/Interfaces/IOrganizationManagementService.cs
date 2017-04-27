using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IOrganizationManagementService
    {
        Task<Organization> GetOrganizationInfo(CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdateOrganizationInfo(Organization organization, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateOrganizationInfo(Organization organization, CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}
