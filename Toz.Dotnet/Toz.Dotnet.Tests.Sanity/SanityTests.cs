using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Toz.Dotnet.Tests.Sanity
{
    public class SanityTests
    {
        private readonly AuthService _authHelper;
        private readonly IPetsManagementService _petsManagementService;
        private readonly INewsManagementService _newsManagementService;
        private readonly IUsersManagementService _userManagementService;
        private readonly IFilesManagementService _filesManagementService;
        private User _testUser;

        public SanityTests()
        {
            _authHelper = new AuthService();
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();
            _filesManagementService = ServiceProvider.Instance.Resolve<IFilesManagementService>();

            _testUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "SanityTest",
                LastName = "User",
                PhoneNumber = "123456789",
                Email = "test@test.com",
                //Roles = UserType.Volunteer
            };

            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
            _newsManagementService.RequestUri = RequestUriHelper.NewsUri;
            _userManagementService.RequestUri = RequestUriHelper.UsersUri;
        }

        [Fact]
        public async void PetsFunctionalityTest()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn()); 
            }
            Assert.True(!string.IsNullOrEmpty(_petsManagementService.RequestUri));
            //Get all pets
            var pets = _petsManagementService.GetAllPets().Result;
            Assert.NotNull(pets);
            //Get specified pet
            if (pets.Any())
            {
                var firstPet = pets.FirstOrDefault();
                var pet = _petsManagementService.GetPet(firstPet.Id).Result;
                Assert.NotNull(pet);
                Assert.Null(_petsManagementService.GetPet("notExistingIDThatIsNotID--1").Result);
                //Update pet
                string petName = firstPet.Name;
                firstPet.Name = "SanityTestName";
                Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
                firstPet.Name = petName;
                Assert.True(_petsManagementService.UpdatePet(firstPet).Result);
                //Delete pet
                Assert.True(_petsManagementService.DeletePet(firstPet).Result);
                //Create pet
                Assert.True(_petsManagementService.CreatePet(firstPet).Result);
            }
        }

        [Fact]
        public async void NewsFunctionalityTest()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            Assert.True(!string.IsNullOrEmpty(_newsManagementService.RequestUri));
            //Get all news
            var news = _newsManagementService.GetAllNews().Result;
            Assert.NotNull(news);
            //Get specified news
            if (news.Any())
            {
                var firstNews = news.FirstOrDefault();
                var singleNews = _newsManagementService.GetNews(firstNews.Id).Result;
                Assert.NotNull(singleNews);
                Assert.Null(_newsManagementService.GetNews("notExistingIDThatIsNotID--1").Result);
                //Update news
                string newsTitle = firstNews.Title;
                firstNews.Title = "SanityTestTitle";
                Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
                firstNews.Title = newsTitle;
                Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
                //Delete news
                Assert.True(_newsManagementService.DeleteNews(firstNews).Result);
                //Create news
                Assert.True(_newsManagementService.CreateNews(firstNews).Result);
            }
        }

        //[Fact]
        //public async void UsersFunctionalityTest()
        //{
        //    if (!_authHelper.AuthHelper.IsAuth)
        //    {
        //        Assert.True(await _authHelper.SignIn());
        //    }
        //    Assert.True(!string.IsNullOrEmpty(_userManagementService.RequestUri));
        //    //Get all users
        //    var users = _userManagementService.GetAllUsers().Result;
        //    Assert.NotNull(users);
        //    //Get specified user
        //    if (users.Any())
        //    {
        //        var firstUser = users.FirstOrDefault();
        //        var singleUser = _userManagementService.GetUser(firstUser.Id).Result;
        //        Assert.NotNull(singleUser);
        //        Assert.Null(_userManagementService.GetUser("notExistingIDThatIsNotID--1").Result);
        //        //Update user
        //        string userFirsName = firstUser.FirstName;
        //        firstUser.FirstName = "SanityTestFirstName";
        //        Assert.True(_userManagementService.UpdateUser(firstUser).Result);
        //        firstUser.FirstName = userFirsName;
        //        Assert.True(_userManagementService.UpdateUser(firstUser).Result);
        //    }
        //    //Create user
        //    Assert.True(_userManagementService.CreateUser(_testUser).Result);
        //    //Delete user
        //    Assert.True(_userManagementService.DeleteUser(_testUser).Result);
        //}

        [Fact]
        public async void FilesFunctionalityTest()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
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
