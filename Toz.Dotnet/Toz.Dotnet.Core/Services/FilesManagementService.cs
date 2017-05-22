using System;
using System.IO;
using System.Net.Http;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Images;
using System.Drawing;

namespace Toz.Dotnet.Core.Services
{
    public class FilesManagementService : IFilesManagementService
    {
        public Image DownloadImage(string address, string token)
        {
            HttpClient httpClient = new HttpClient();
            var imgBytes = httpClient.GetStreamAsync(address);
            return Image.FromStream(imgBytes.Result);
        }

        public bool UploadImage(Image image, string token)
        {
            //todo: use a magic craft to send image to the backend.
            return true;
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
    }
}