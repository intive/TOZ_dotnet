using System.Drawing;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IFilesManagementService
    {
         bool UploadImage(Image image);
         Image DownloadImage(string jsonAddress);
    }
}