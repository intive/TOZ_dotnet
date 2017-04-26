using System;
using System.IO;
using System.Net.Http;
using ImageSharp;
using ImageSharp.PixelFormats;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Images;
using Image = System.Drawing.Image;

namespace Toz.Dotnet.Core.Services
{
    public class FilesManagementService : IFilesManagementService
    {
        public Image DownloadImage(string address)
        {
            HttpClient httpClient = new HttpClient();
            var imgBytes = httpClient.GetStreamAsync(address);
            return Image.FromStream(imgBytes.Result);
        }

        public bool UploadImage(Image image)
        {
            //todo: use a magic craft to send image to the backend.
            return true;
        }

        public Image<Rgba32> GetThumbnail(Image image) // ImageSharp library
        {
            var img = ImageSharp.Image.Load(Configuration.Default, ImageToByteArray(image));
            return img.Resize(Thumbnails.Width, Thumbnails.Height);
        }

        public Image GetThumbnail2(Image image) // Just System.Drawing
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