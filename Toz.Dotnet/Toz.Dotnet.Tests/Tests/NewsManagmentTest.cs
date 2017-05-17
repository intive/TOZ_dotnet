using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Toz.Dotnet.Tests.Tests
{
    public class NewsManagementTest
    {
        private readonly INewsManagementService _newsManagementService;
        private readonly News _testingNews;

        public NewsManagementTest()
        {
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
            Assert.NotNull(await _newsManagementService.GetAllNews());
        }
         
        [Fact]
        public async void TestOfCreatingNewNews()
        {
            Assert.True(await _newsManagementService.CreateNews(_testingNews));
        }

        [Fact]
        public async void TestOfDeletingSpecifiedNews()
        {
            Assert.True(await _newsManagementService.DeleteNews(_testingNews));
        }

        [Fact]
        public async void TestOfGettingSpecifiedNews()
        {
            Assert.NotNull(await _newsManagementService.GetNews(_testingNews.Id));         
        }

        
        [Fact]
        public async void TestOUpdatingNews()
        {
           Assert.True(await _newsManagementService.UpdateNews(_testingNews));
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

        [Theory]
        [InlineData("Contents")]
        [InlineData("Title")]
        public void TestPetValidationIfLengthIsNotValid(string propertyName)
        {
            const int maxLength = 1000;
            const int minLength = 1;

            var news = CloneNews(_testingNews);
            var property = news.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            Assert.NotNull(property);

            property.SetValue(news, new string('A', maxLength + 1));

            var context = new ValidationContext(news, null, null);
            var result = new List<ValidationResult>();
            bool validMax = Validator.TryValidateObject(news, context, result, true);
            property.SetValue(news, new string('A', minLength -1));
            bool validMin = Validator.TryValidateObject(news, context, result, true);

            Assert.False(validMax);
            Assert.False(validMin);
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
