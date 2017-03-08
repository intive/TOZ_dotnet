using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Xunit;
using System.Drawing;

namespace Toz.Dotnet.Tests.Tests
{
    public class FilesManagementServiceTest
    {
        private IFilesManagementService _filesManagementService;
        private const string ImageAddress = "here will be address that points to some image";

        public FilesManagementServiceTest()
        {
            _filesManagementService = ServiceProvider.Instance.Resolve<IFilesManagementService>();
        }

        [Fact]
        public void TestImageDownloading()
        {
            Assert.NotNull(_filesManagementService.DownloadImage(ImageAddress));
        }

        [Fact]
        public void TestImageUploading()
        {
            Bitmap bitmap = new Bitmap(100,100);
            Assert.True(_filesManagementService.UploadImage(bitmap));
        }
    }
}