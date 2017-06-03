using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class CommentsManagementService : ICommentsManagementService
    {
        private readonly IRestService _restService;
        public string RequestUri { get; set; }

        public CommentsManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            RequestUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendCommentsUrl;
        }

        public async Task<List<Comment>> GetAllComments(string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = RequestUri;
            return await _restService.ExecuteGetAction<List<Comment>>(address, token, cancelationToken);
        }

        public async Task<Comment> GetComment(string id, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Comment>(address, token, cancelationToken);
        }

        public async Task<bool> DeleteComment(string id, string token, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{id}";
            return await _restService.ExecuteDeleteAction(address, token, cancelationToken);
        }
    }
}
