using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models.Organization;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IOrganizationManagementService
    {
        Task<bool> UpdateOrCreateInfo(Organization organizationInfo, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<Organization> GetOrganizationInfo(string token, CancellationToken cancellationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}