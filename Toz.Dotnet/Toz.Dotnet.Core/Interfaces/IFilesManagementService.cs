using System.Drawing;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IFilesManagementService
    {
        bool UploadImage(Image image, string token);
        Image DownloadImage(string address, string token);
        Image GetThumbnail(Image image);
        byte[] ImageToByteArray(Image image);
        Image ByteArrayToImage(byte[] bytes);
    }
}