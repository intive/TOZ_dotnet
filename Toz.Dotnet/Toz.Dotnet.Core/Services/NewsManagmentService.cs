using Toz.Dotnet.Core.Interfaces;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;
using System.Threading;

namespace Toz.Dotnet.Core.Services
{
    public class NewsManagementService : INewsManagementService
    {
        private IRestService _restService;
        private IFilesManagementService _filesManagementService;
        public string RequestUri { get; set; }

        public NewsManagementService(IFilesManagementService filesManagementService, IRestService restService, IOptions<AppSettings> appSettings)
        {
            _filesManagementService = filesManagementService;
            _restService = restService;

            RequestUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendNewsUrl;
        }

        public async Task<List<News>> GetAllNews(CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = RequestUri;
            return await _restService.ExecuteGetAction<List<News>>(address, cancelationToken);
        }
		
        
        public async Task<bool> UpdateNews(News news, CancellationToken cancelationToken = default(CancellationToken))
        {
           var address = $"{RequestUri}/{news.Id}";
           return await _restService.ExecutePutAction(address, news, cancelationToken);
        }

        
        public async Task<bool> CreateNews(News news, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, news, cancelationToken);
        }

        public async Task<bool> DeleteNews(News news, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{news.Id}";
            return await _restService.ExecuteDeleteAction(address, cancelationToken);
        }

        public async Task<News> GetNews(string id, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<News>(address, cancelationToken);
        }

        public byte[] ConvertPhotoToByteArray(Stream fileStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public Task<bool> UploadImage(News news, CancellationToken cancelationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }
    }
}