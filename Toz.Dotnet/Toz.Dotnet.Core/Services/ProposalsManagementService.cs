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
        private readonly AppSettings _appSettings;
        public string RequestUri { get; set; }

        public ProposalsManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            _appSettings = appSettings.Value;
            RequestUri = appSettings.Value.BackendProposalsUrl;
        }

        public async Task<Proposal> GetProposal(string id, CancellationToken cancelationToken = new CancellationToken())
        {
            var proposals =await GetAllProposals(cancelationToken);
            return proposals.Find(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<Proposal>> GetAllProposals(CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecuteGetAction<List<Proposal>>(address, cancelationToken);
        }

        public async Task<bool> CreateProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, proposal, cancellationToken);
        }

        public async Task<bool> UpdateProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{proposal.Id}";
            return await _restService.ExecutePutAction(address, proposal, cancellationToken);
        }

        public async Task<bool> DeleteProposal(Proposal proposal, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{proposal.Id}";
            return await _restService.ExecuteDeleteAction(address, cancellationToken);
        }

        public async Task<bool> SendActivationEmail(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            var address = $"{_appSettings.BackendActivationUserUrl}/{id}";
            var proposal =  await _restService.ExecuteGetAction<Proposal>(address, cancellationToken);
            if (proposal == null)
            {
                return false;
            }
            return await UpdateProposal(proposal, cancellationToken);
        }
    }
}
