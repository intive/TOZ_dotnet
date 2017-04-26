using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IOrganizationInfoManagementService
    {
        Task<bool> UpdateOrCreateInfo(OrganizationInfo organizationInfo, CancellationToken cancelationToken = default(CancellationToken));
        Task<OrganizationInfo> GetOrganizationInfo(CancellationToken cancellationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}