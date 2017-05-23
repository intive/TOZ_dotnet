using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IProposalsManagementService
    {
        Task<bool> DeleteProposal(Proposal proposal, string token, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> UpdateProposal(Proposal proposal, string token, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateProposal(Proposal proposal, string token, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Proposal>> GetAllProposals(string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<Proposal> GetProposal(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
        string ActivationRequestUri { get; set; }
        Task<bool> SendActivationEmail(string id, string token, CancellationToken cancellationToken = default(CancellationToken));
    }
}