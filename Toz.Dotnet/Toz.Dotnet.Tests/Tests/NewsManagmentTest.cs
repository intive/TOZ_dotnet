using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toz.Dotnet.Tests.Tests
{
    public class NewsManagementTest
    {
        private INewsManagementService _newsManagementService;
        private News _testingNews;
        public NewsManagementTest()
        {
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();
            _testingNews = new News()
            {               
                Id = System.Guid.NewGuid().ToString(),
                Title = "TestNews",
                PublishingTime = DateTime.Now,
                AddingTime = DateTime.Now,
                LastEditTime = DateTime.Now,
                Body = "Text",
                Photo = new byte[10],
                Status = NewsStatus.Published
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
        public void TestOfGettingAllNews()
        {
            Assert.NotNull(_newsManagementService.GetAllNews().Result);
        }
         
        [Fact]
        public void TestOfCreatingNewNews()
        {
            Assert.True(_newsManagementService.CreateNews(_testingNews).Result);
            _newsManagementService.DeleteNews(_testingNews).Wait();
        }

        [Fact]
        public void TestOfDeletingSpecifiedNews()
        {
            var news = _newsManagementService.GetAllNews().Result;
            if(news.Any())
            {
                var firstNews = news.FirstOrDefault();
                Assert.True(_newsManagementService.DeleteNews(firstNews).Result);
                Assert.True(_newsManagementService.CreateNews(firstNews).Result);
            }
        }

        [Fact]
        public void TestOfGettingSpecifiedNews()
        {
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
        public void TestOfNewsUpdating()
        {
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
            return new News()
            {
                Id = news.Id,
                Title = news.Title,
                PublishingTime = news.PublishingTime,
                AddingTime = news.AddingTime,
                LastEditTime = news.LastEditTime,
                Body = news.Body,
                Photo = news.Photo,
                Status = news.Status
            };
        }
        
    }
}