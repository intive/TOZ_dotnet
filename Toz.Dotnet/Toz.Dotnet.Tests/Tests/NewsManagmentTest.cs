using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Tests.Tests
{
    public class NewsManagementTest
    {
        private readonly AuthService _authHelper;
        private readonly INewsManagementService _newsManagementService;
        private readonly News _testingNews;

        public NewsManagementTest()
        {
            _authHelper = new AuthService();
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();

            _testingNews = new News
            {               
                Id = Guid.NewGuid().ToString(),
                Title = "TestNews",
                Published = DateTime.Now,
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                Contents = "Text",
                Photo = new byte[10],
                Type = NewsStatus.Released
            };

            _newsManagementService.RequestUri = RequestUriHelper.NewsUri;
        }
        
        [Fact]
        public void TestDependencyInjectionFromNewsManagementService()
        {
            Assert.NotNull(_newsManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_newsManagementService.RequestUri));
        }

        [Fact]
        public async void TestOfGettingAllNews()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            Assert.NotNull(_newsManagementService.GetAllNews().Result);
        }
         
        [Fact]
        public async void TestOfCreatingNewNews()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            Assert.True(_newsManagementService.CreateNews(_testingNews).Result);
            _newsManagementService.DeleteNews(_testingNews).Wait();
        }

        [Fact]
        public async void TestOfDeletingSpecifiedNews()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            var news = _newsManagementService.GetAllNews().Result;
            if(news.Any())
            {
                var firstNews = news.FirstOrDefault();
                Assert.True(_newsManagementService.DeleteNews(firstNews).Result);
                Assert.True(_newsManagementService.CreateNews(firstNews).Result);
            }
        }

        [Fact]
        public async void TestOfGettingSpecifiedNews()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            var news = _newsManagementService.GetAllNews().Result;
            if(news.Any())
            {
                var firstNews = news.FirstOrDefault();
                var singleNews = _newsManagementService.GetNews(firstNews.Id).Result;
                Assert.NotNull(singleNews);
            }
                       
            Assert.Null(_newsManagementService.GetNews("notExistingIDThatIsNotID--1").Result);           
        }

        
        [Fact]
        public async void TestOfNewsUpdating()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            var news = _newsManagementService.GetAllNews().Result;
            if(news.Any())
            {
                var firstNews = news.FirstOrDefault();
                string newsTitle = firstNews.Title;
                firstNews.Title = "Test";
                Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
                firstNews.Title = newsTitle;
                Assert.True(_newsManagementService.UpdateNews(firstNews).Result);
            }
        }
        
        [Fact]
        public void TestNewsValidationIfCorrectData()
        {
            // Arrange
            var context = new ValidationContext(_testingNews, null, null);
            var result = new List<ValidationResult>();

            // Act
            bool valid = Validator.TryValidateObject(_testingNews, context, result, true);

            Assert.True(valid);
        }

        private News CloneNews(News news)
        {
            return new News
            {
                Id = news.Id,
                Title = news.Title,
                Published = news.Published,
                Created = news.Created,
                LastModified = news.LastModified,
                Contents = news.Contents,
                Photo = news.Photo,
                Type = news.Type
            };
        }
        
    }
}
