using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using System;

namespace Toz.Dotnet.Tests.Sanity
{
    public class SanityTests
    {
        private readonly IPetsManagementService _petsManagementService;
        private readonly INewsManagementService _newsManagementService;
        private readonly IUsersManagementService _userManagementService;
        private readonly IFilesManagementService _filesManagementService;
        private readonly IOrganizationManagementService _organizationManagementService;

        public SanityTests()
        {
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();
            _filesManagementService = ServiceProvider.Instance.Resolve<IFilesManagementService>();
            _organizationManagementService = ServiceProvider.Instance.Resolve<IOrganizationManagementService>();

            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
            _newsManagementService.RequestUri = RequestUriHelper.NewsUri;
            _userManagementService.RequestUri = RequestUriHelper.UsersUri;
            _organizationManagementService.RequestUri = RequestUriHelper.OrganizationInfoUri;
        }

        [Fact]
        public async void PetsFunctionalityTest()
        {
            var pet = TestingObjectProvider.Instance.Pet;

            Assert.NotNull(await _petsManagementService.GetAllPets());
            Assert.NotNull(await _petsManagementService.GetPet(pet.Id));
            Assert.True(await _petsManagementService.CreatePet(pet));
            Assert.True(await _petsManagementService.UpdatePet(pet));

            var exception = Record.Exception(() => _petsManagementService.UpdatePet(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);

            Assert.False(await _petsManagementService.CreatePet(null));
        }

        [Fact]
        public async void NewsFunctionalityTest()
        {
            var news = TestingObjectProvider.Instance.News;

            Assert.NotNull(await _newsManagementService.GetAllNews());
            Assert.NotNull(await _newsManagementService.GetNews(news.Id));
            Assert.True(await _newsManagementService.CreateNews(news));
            Assert.True(await _newsManagementService.DeleteNews(news));

            var exception = Record.Exception(() => _newsManagementService.UpdateNews(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);

            Assert.False(await _newsManagementService.CreateNews(null));
        }

        [Fact]
        public async void UsersFunctionalityTest()
        {
            var user = TestingObjectProvider.Instance.User;

            Assert.NotNull(await _userManagementService.GetAllUsers());
            Assert.NotNull(await _userManagementService.GetUser(user.Id));
            Assert.True(await _userManagementService.CreateUser(user));
            Assert.True(await _userManagementService.DeleteUser(user));

            var exception = Record.Exception(() => _userManagementService.UpdateUser(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);

            Assert.False(await _userManagementService.CreateUser(null));
        }

        [Fact]
        public async void OrganizationFunctionalityTest()
        {
            var organization = TestingObjectProvider.Instance.Organization;

            Assert.NotNull(await _organizationManagementService.GetOrganizationInfo());
            Assert.True(await _organizationManagementService.UpdateOrCreateInfo(organization));
        }

        [Fact]
        public void FilesFunctionalityTest()
        {
            //Download image
            var img = _filesManagementService.DownloadImage(@"http://i.pinger.pl/pgr167/7dc36d63001e9eeb4f01daf3/kot%20ze%20shreka9.jpg");
            Assert.NotNull(img);
            //Get thumbnail
            Assert.NotNull(_filesManagementService.GetThumbnail(img));
            //Image to byte array
            Assert.NotNull(_filesManagementService.ImageToByteArray(img));
            //Byte array to image
            var imgBytes = _filesManagementService.ImageToByteArray(img);
            Assert.NotNull(_filesManagementService.ByteArrayToImage(imgBytes));
        }
    }
}
