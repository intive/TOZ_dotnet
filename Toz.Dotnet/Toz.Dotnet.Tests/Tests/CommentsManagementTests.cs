using System;
using System.Collections.Generic;
using System.Text;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class CommentsManagementTests
    {
        private readonly Comment _testingComment;
        private readonly ICommentsManagementService _commentsManagementService;
        private readonly IAccountManagementService _accountManagementService;
        private readonly JwtToken _token;

        public CommentsManagementTests()
        {
            _commentsManagementService = ServiceProvider.Instance.Resolve<ICommentsManagementService>();
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _commentsManagementService.RequestUri = RequestUriHelper.CommentsUri;
            _accountManagementService.RequestUriJwt = RequestUriHelper.JwtTokenUri;
            _testingComment = TestingObjectProvider.Instance.Comment;
            _token = _accountManagementService.SignIn(TestingObjectProvider.Instance.Login).Result;
        }

        [Fact]
        public void TestDependencyInjection()
        {
            Assert.NotNull(_commentsManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_commentsManagementService.RequestUri));
        }

        [Fact]
        public void TestOfGettingAllComments()
        {
            Assert.NotNull(_commentsManagementService.GetAllComments(_token.Jwt).Result);
        }

        [Fact]
        public void TestOfDeletingSpecifiedComment()
        {
            Assert.True(_commentsManagementService.DeleteComment(_testingComment.Id, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfGettingSpecifiedComment()
        {
            Assert.NotNull(_commentsManagementService.GetComment(_testingComment.Id, _token.Jwt).Result);
        }

        [Fact]
        public void TestOfGettingAllCommentsUsingWrongUrl()
        {
            _commentsManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_commentsManagementService.GetAllComments(_token.Jwt).Result);
            _commentsManagementService.RequestUri = RequestUriHelper.PetsUri;
        }
    }
}
