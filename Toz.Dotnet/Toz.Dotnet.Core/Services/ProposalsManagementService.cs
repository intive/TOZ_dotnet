using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class ProposalsManagementService : IProposalsManagementService
    {
        private readonly IRestService _restService;
        public string RequestUri { get; set; }

        public ProposalsManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendProposalsUrl;
        }

        public async Task<List<Proposal>> GetAllProposals(CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecuteGetAction<List<Proposal>>(address, cancelationToken);
        }

        public async Task<bool> AddProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, proposal, cancellationToken);
        }

        public async Task<bool> UpdateProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePutAction(address, proposal, cancellationToken);
        }

        public async Task<bool> DeleteProposal(string proposalId,CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{proposalId}";
            return await _restService.ExecuteDeleteAction(address, cancellationToken);
        }
    }
}
