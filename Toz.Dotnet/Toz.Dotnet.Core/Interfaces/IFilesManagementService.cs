using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IFilesManagementService
    {
        Image DownloadImage(string address);
        Image GetThumbnail(Image image);
        byte[] ImageToByteArray(Image image);
        Image ByteArrayToImage(byte[] bytes);
        Task<bool> UploadPetAvatar(string id, string token, IEnumerable<IFormFile> files, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UploadNewsAvatar(string id, string token, IEnumerable<IFormFile> files, CancellationToken cancelationToken = default(CancellationToken));


    }
}