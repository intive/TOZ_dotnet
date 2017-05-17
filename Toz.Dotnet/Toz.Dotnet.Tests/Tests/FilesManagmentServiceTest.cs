using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Xunit;
using System.Drawing;
using Toz.Dotnet.Models.Images;

namespace Toz.Dotnet.Tests.Tests
{
    public class FilesManagementServiceTest
    {
        private readonly IFilesManagementService _filesManagementService;
        private const string ImageAddress = @"http://i.pinger.pl/pgr167/7dc36d63001e9eeb4f01daf3/kot%20ze%20shreka9.jpg";

        public FilesManagementServiceTest()
        {
            _filesManagementService = ServiceProvider.Instance.Resolve<IFilesManagementService>();
        }

        [Fact]
        public void DownloadImageTest()
        {
            Assert.NotNull(_filesManagementService.DownloadImage(ImageAddress));
        }

        [Fact]
        public void UploadImageTest()
        {
            Bitmap bitmap = new Bitmap(100, 100);
            Assert.True(_filesManagementService.UploadImage(bitmap));
        }

        [Fact]
        public void GetThumbnailsTest()
        {
            Assert.NotNull(_filesManagementService.GetThumbnail(_filesManagementService.DownloadImage(ImageAddress)));
        }

        [Fact]
        public void ProperThumbnailSizesTest()
        {
            Image thumbnail = _filesManagementService.GetThumbnail(_filesManagementService.DownloadImage(ImageAddress));
            Assert.Equal(Thumbnails.Width, thumbnail.Width);
            Assert.Equal(Thumbnails.Height, thumbnail.Height);
        }

        [Fact]
        public void ImageToByteArrayTest()
        {
            Image img = _filesManagementService.DownloadImage(ImageAddress);
            Assert.NotNull(_filesManagementService.ImageToByteArray(img));
        }

        [Fact]
        public void ByteArrayToImageTest()
        {
            byte[] imgBytes = _filesManagementService.ImageToByteArray(_filesManagementService.DownloadImage(ImageAddress));
            Assert.NotNull(_filesManagementService.ByteArrayToImage(imgBytes));
        }
    }
}
