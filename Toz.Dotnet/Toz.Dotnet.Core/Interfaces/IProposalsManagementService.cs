using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IProposalsManagementService
    {
        Task<bool> DeleteProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> UpdateProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Proposal>> GetAllProposals(CancellationToken cancelationToken = default(CancellationToken));
        Task<Proposal> GetProposal(string id, CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}