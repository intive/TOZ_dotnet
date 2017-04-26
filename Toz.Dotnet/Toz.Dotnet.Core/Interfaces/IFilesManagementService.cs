using ImageSharp;
using ImageSharp.PixelFormats;
using Image = System.Drawing.Image;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IFilesManagementService
    {
        bool UploadImage(Image image);
        Image DownloadImage(string jsonAddress);
        Image<Rgba32> GetThumbnail(Image image);
        Image GetThumbnail2(Image image);
        byte[] ImageToByteArray(Image image);
        Image ByteArrayToImage(byte[] bytes);
    }
}