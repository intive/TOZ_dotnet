using System.Drawing;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IFilesManagementService
    {
        Image DownloadImage(string address);
        Image GetThumbnail(Image image);
        byte[] ImageToByteArray(Image image);
        Image ByteArrayToImage(byte[] bytes);
    }
}