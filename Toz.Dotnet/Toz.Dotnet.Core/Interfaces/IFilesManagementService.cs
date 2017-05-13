using System.Drawing;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IFilesManagementService
    {
        bool UploadImage(Image image);
        Image DownloadImage(string address);
        Image GetThumbnail(Image image);
        byte[] ImageToByteArray(Image image);
        Image ByteArrayToImage(byte[] bytes);
    }
}