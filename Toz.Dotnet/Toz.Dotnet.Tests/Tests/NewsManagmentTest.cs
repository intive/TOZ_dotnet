using System;
using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using Toz.Dotnet.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Toz.Dotnet.Tests.Tests
{
    public class NewsManagementTest
    {
        private readonly INewsManagementService _newsManagementService;
        private readonly News _testingNews;
        private readonly IAccountManagementService _accountManagementService;
        private readonly JwtToken _token;

        public NewsManagementTest()
        {
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _newsManagementService.RequestUri = RequestUriHelper.NewsUri;
            _accountManagementService.RequestUri = RequestUriHelper.JwtTokenUrl;
            _testingNews = TestingObjectProvider.Instance.News;
            _token = _accountManagementService.SignIn(TestingObjectProvider.Instance.Login).Result;
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
            Assert.NotNull(await _newsManagementService.GetAllNews(_token.Jwt));
        }
         
        [Fact]
        public async void TestOfCreatingNewNews()
        {
            Assert.True(await _newsManagementService.CreateNews(_testingNews, _token.Jwt));
        }

        [Fact]
        public async void TestOfDeletingSpecifiedNews()
        {
            Assert.True(await _newsManagementService.DeleteNews(_testingNews, _token.Jwt));
        }

        [Fact]
        public async void TestOfGettingSpecifiedNews()
        {
            Assert.NotNull(await _newsManagementService.GetNews(_testingNews.Id, _token.Jwt));         
        }

        
        [Fact]
        public async void TestOUpdatingNews()
        {
           Assert.True(await _newsManagementService.UpdateNews(_testingNews, _token.Jwt));
        }

        [Fact]
        public void TestOfUpdatingPetWithNullValue()
        {
            var exception = Record.Exception(() => _newsManagementService.UpdateNews(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfDeletingPetThatIsNull()
        {
            var exception = Record.Exception(() => _newsManagementService.DeleteNews(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllPetsUsingWrongUrl()
        {
            _newsManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_newsManagementService.GetAllNews(_token.Jwt).Result);
            _newsManagementService.RequestUri = RequestUriHelper.PetsUri;
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

            var news = TestingObjectProvider.Instance.DoShallowCopy(_testingNews);
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
        
    }
}
