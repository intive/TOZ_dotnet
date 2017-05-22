using System;
using System.Collections.Generic;
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
        public string ActivationRequestUri { get; set; }

        public ProposalsManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendProposalsUrl;
            ActivationRequestUri = appSettings.Value.BackendActivationUserUrl;
        }

        public async Task<Proposal> GetProposal(string id, string token, CancellationToken cancelationToken = new CancellationToken())
        {
            var proposals = await GetAllProposals(token, cancelationToken);
            return proposals.Find(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<Proposal>> GetAllProposals(string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecuteGetAction<List<Proposal>>(address, token, cancelationToken);
        }

        public async Task<bool> CreateProposal(Proposal proposal, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, proposal, token, cancellationToken);
        }

        public async Task<bool> UpdateProposal(Proposal proposal, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{proposal.Id}";
            return await _restService.ExecutePutAction(address, proposal, token, cancellationToken);
        }

        public async Task<bool> DeleteProposal(Proposal proposal, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{proposal.Id}";
            return await _restService.ExecuteDeleteAction(address, token, cancellationToken);
        }

        public async Task<bool> SendActivationEmail(string id, string token, CancellationToken cancellationToken = new CancellationToken())
        {
            var address = $"{ActivationRequestUri}/{id}";
            var proposal =  await _restService.ExecuteGetAction<Proposal>(address, token, cancellationToken);
            if (proposal == null)
            {
                return false;
            }
            return await UpdateProposal(proposal, token, cancellationToken);
        }
    }
}
