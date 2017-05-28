using System;
using System.IO;
using System.Net.Http;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Images;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;
using System.Threading.Tasks;

namespace Toz.Dotnet.Core.Services
{
    public class FilesManagementService : IFilesManagementService
    {
        private readonly IRestService _restService;
        public string PetAvatarUri { get; set; }
        public string NewsAvatarUri { get; set; }

        public FilesManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            PetAvatarUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendPetsUrl + "/{id}" + appSettings.Value.BackendImagesUrl;
            NewsAvatarUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendNewsUrl + "/{id}" + appSettings.Value.BackendImagesUrl;
        }

        public Image DownloadImage(string address)
        {
            HttpClient httpClient = new HttpClient();
            var imgBytes = httpClient.GetStreamAsync(address);
            return Image.FromStream(imgBytes.Result);
        }

        public Image GetThumbnail(Image image)
        {
            var thumbnail = image.GetThumbnailImage(Thumbnails.Width, Thumbnails.Height, () => true, new IntPtr());
            return thumbnail;
        }

        public byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.ToArray();
            }
        }

        public Image ByteArrayToImage(byte[] bytes)
        {
            using (var mStream = new MemoryStream(bytes))
            {
                return Image.FromStream(mStream);
            }
        }

        public async Task<bool> UploadPetAvatar(string id, string token, IEnumerable<IFormFile> files, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = PetAvatarUri.Replace("{id}", id);
            return await _restService.ExecutePostMultipartAction(address, files, id, token, cancelationToken);
        }

        public async Task<bool> UploadNewsAvatar(string id, string token, IEnumerable<IFormFile> files, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = NewsAvatarUri.Replace("{id}", id);
            return await _restService.ExecutePostMultipartAction(address, files, id, token, cancelationToken);
        }
    }
}