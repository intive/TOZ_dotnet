using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IOrganizationInfoManagementService
    {
        Task<bool> UpdateOrCreateInfo(Organization organizationInfo, CancellationToken cancelationToken = default(CancellationToken));
        Task<Organization> GetOrganizationInfo(CancellationToken cancellationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}