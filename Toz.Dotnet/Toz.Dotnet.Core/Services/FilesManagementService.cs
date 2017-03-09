using System.Drawing;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Core.Services
{
    public class FilesManagementService : IFilesManagementService
    {
        public Image DownloadImage(string jsonAddress)
        {
            Bitmap bitmap = new Bitmap(100, 100);
            return bitmap;
        }

        public bool UploadImage(Image image)
        {
            //todo: use a magic craft to send image to the backend.
            return true;
        }
    }
}