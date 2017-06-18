using Xunit;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Options;
using Moq;
using Toz.Dotnet.Core.Services;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Tests.Sanity
{
    public class SanityTests : IDisposable
    {
        private readonly IPetsManagementService _petsManagementService;
        private readonly IPetsStatusManagementService _petsStatusManagementService;
        private readonly INewsManagementService _newsManagementService;
        private readonly IUsersManagementService _userManagementService;
        private readonly IFilesManagementService _filesManagementService;
        private readonly IOrganizationManagementService _organizationManagementService;
        private readonly IProposalsManagementService _proposalsManagementService;
        private readonly IHowToHelpInformationService _howToHelpInformationService;
        private readonly IAccountManagementService _accountManagementService;
        private readonly IHelpersManagementService _helpersManagementService;
        private readonly ICommentsManagementService _commentsManagementService;
        private JwtToken _token;

        public SanityTests()
        {
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
            _petsStatusManagementService = ServiceProvider.Instance.Resolve<IPetsStatusManagementService>();
            _newsManagementService = ServiceProvider.Instance.Resolve<INewsManagementService>();
            _userManagementService = ServiceProvider.Instance.Resolve<IUsersManagementService>();
            _filesManagementService = ServiceProvider.Instance.Resolve<IFilesManagementService>();
            _organizationManagementService = ServiceProvider.Instance.Resolve<IOrganizationManagementService>();
            _proposalsManagementService = ServiceProvider.Instance.Resolve<IProposalsManagementService>();
            _howToHelpInformationService = ServiceProvider.Instance.Resolve<IHowToHelpInformationService>();
            _accountManagementService = ServiceProvider.Instance.Resolve<IAccountManagementService>();
            _helpersManagementService = ServiceProvider.Instance.Resolve<IHelpersManagementService>();
            _commentsManagementService = ServiceProvider.Instance.Resolve<ICommentsManagementService>();

            _petsManagementService.RequestUri = RequestUriHelper.PetsUri;
            _petsStatusManagementService.RequestUri = RequestUriHelper.PetsStatusUri;
            _newsManagementService.RequestUri = RequestUriHelper.NewsUri;
            _userManagementService.RequestUri = RequestUriHelper.UsersUri;
            _organizationManagementService.RequestUri = RequestUriHelper.OrganizationInfoUri;
            _proposalsManagementService.RequestUri = RequestUriHelper.ProposalsUri;
            _howToHelpInformationService.BecomeVolunteerUrl = RequestUriHelper.HowToHelpUri;
            _howToHelpInformationService.DonateInfoUrl = RequestUriHelper.HowToHelpUri;
            _accountManagementService.RequestUriJwt = RequestUriHelper.JwtTokenUri;
            _helpersManagementService.RequestUri = RequestUriHelper.HelpersUri;
            _commentsManagementService.RequestUri = RequestUriHelper.CommentsUri;
        }

        [Fact]
        public async void PetsFunctionalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var pet = TestingObjectProvider.Instance.Pet;

            Assert.NotNull(await _petsManagementService.GetAllPets(_token.Jwt));
            Assert.NotNull(await _petsManagementService.GetPet(pet.Id, _token.Jwt));
            Assert.True(await _petsManagementService.CreatePet(pet, _token.Jwt));
            Assert.True(await _petsManagementService.UpdatePet(pet, _token.Jwt));

            var exception = Record.Exception(() => _petsManagementService.UpdatePet(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);

            Assert.False(await _petsManagementService.CreatePet(null, _token.Jwt));
        }

        [Fact]
        public async void PetsStatusFunctionalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var petStatus = TestingObjectProvider.Instance.PetsStatus;

            Assert.NotNull(await _petsStatusManagementService.GetAllStatus(_token.Jwt));
            Assert.True(await  _petsStatusManagementService.CreateStatus(petStatus, _token.Jwt));
            Assert.True(await _petsStatusManagementService.DeleteStatus(petStatus, _token.Jwt));
            Assert.True(await _petsStatusManagementService.UpdateStatus(petStatus, _token.Jwt));
        }

        [Fact]
        public async void AccountFunctionalityTest()
        {
            var login = TestingObjectProvider.Instance.Login;

            Assert.NotNull(await _accountManagementService.SignIn(login));
            Assert.Null(await _accountManagementService.SignIn(null));
        }

        [Fact]
        public async void NewsFunctionalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var news = TestingObjectProvider.Instance.News;

            Assert.NotNull(await _newsManagementService.GetAllNews(_token.Jwt));
            Assert.NotNull(await _newsManagementService.GetNews(news.Id, _token.Jwt));
            Assert.True(await _newsManagementService.CreateNews(news, _token.Jwt));
            Assert.True(await _newsManagementService.DeleteNews(news, _token.Jwt));

            var exception = Record.Exception(() => _newsManagementService.UpdateNews(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);

            Assert.False(await _newsManagementService.CreateNews(null, _token.Jwt));
        }

        [Fact]
        public async void UsersFunctionalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var user = TestingObjectProvider.Instance.User;

            Assert.NotNull(await _userManagementService.GetAllUsers(_token.Jwt));
            Assert.NotNull(await _userManagementService.GetUser(user.Id, _token.Jwt));
            Assert.True(await _userManagementService.CreateUser(user, _token.Jwt));
            Assert.True(await _userManagementService.DeleteUser(user, _token.Jwt));

            var exception = Record.Exception(() => _userManagementService.UpdateUser(null, _token.Jwt).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);

            Assert.False(await _userManagementService.CreateUser(null, _token.Jwt));
        }

        [Fact]
        public async void OrganizationFunctionalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var organization = TestingObjectProvider.Instance.Organization;

            Assert.NotNull(await _organizationManagementService.GetOrganizationInfo(_token.Jwt));
            Assert.True(await _organizationManagementService.UpdateOrCreateInfo(organization, _token.Jwt));
        }

        [Fact]
        public void FilesFunctionalityTest()
        {
            //Download image
            var img = _filesManagementService.DownloadImage(
                @"http://i.pinger.pl/pgr167/7dc36d63001e9eeb4f01daf3/kot%20ze%20shreka9.jpg");
            Assert.NotNull(img);
            //Get thumbnail
            Assert.NotNull(_filesManagementService.GetThumbnail(img));
            //Image to byte array
            Assert.NotNull(_filesManagementService.ImageToByteArray(img));
            //Byte array to image
            var imgBytes = _filesManagementService.ImageToByteArray(img);
            Assert.NotNull(_filesManagementService.ByteArrayToImage(imgBytes));
        }

        [Fact]
        public async void ProposalsFunctionalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var proposal = TestingObjectProvider.Instance.Proposal;

            var restServiceMock = new Mock<IRestService>();
            restServiceMock.Setup(s => s.ExecuteGetAction<List<Proposal>>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Proposal>() { proposal });

            restServiceMock.Setup(s => s.ExecutePostAction<ActivationMessage>(
                    It.IsAny<string>(),
                    It.IsAny<ActivationMessage>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            restServiceMock.Setup(s => s.ExecutePutAction<Proposal>(
                    It.IsAny<string>(),
                    It.IsAny<Proposal>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var options = ServiceProvider.Instance.Resolve<IOptions<AppSettings>>();
            var proposalManagementService =
                new ProposalsManagementService(restServiceMock.Object, options)
                {
                    RequestUri = RequestUriHelper.ProposalsUri,
                    ActivationRequestUri = RequestUriHelper.ProposalsUri
                };

            Assert.NotNull(_proposalsManagementService.CreateProposal(proposal, _token.Jwt).Result);
            Assert.True(_proposalsManagementService.DeleteProposal(proposal, _token.Jwt).Result);
            Assert.True(_proposalsManagementService.UpdateProposal(proposal, _token.Jwt).Result);
            Assert.NotNull(proposalManagementService.RequestUri);
            Assert.NotNull(proposalManagementService.ActivationRequestUri);
            Assert.True(proposalManagementService.SendActivationEmail(proposal.Id, _token.Jwt).Result);
            Assert.NotNull(proposalManagementService.GetProposal(proposal.Id, _token.Jwt).Result);
        }

        [Fact]
        public async void HowToHelpFunctionalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var info = TestingObjectProvider.Instance.HowToHelpInfo;

            Assert.NotNull(await _howToHelpInformationService.GetHelpInfo(HowToHelpInfoType.BecomeVolunteer, _token.Jwt));
            Assert.NotNull(await _howToHelpInformationService.GetHelpInfo(HowToHelpInfoType.Donate, _token.Jwt));
            Assert.True(await _howToHelpInformationService.UpdateOrCreateHelpInfo(info, HowToHelpInfoType.BecomeVolunteer, _token.Jwt));
            Assert.True(await _howToHelpInformationService.UpdateOrCreateHelpInfo(info, HowToHelpInfoType.Donate, _token.Jwt));
        }

        [Fact]
        public async void HeleprsFunctinalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);

            Assert.NotNull(_token);

            var helper = TestingObjectProvider.Instance.Helper;

            Assert.NotNull(_helpersManagementService.GetAllHelpers(_token.Jwt).Result);
            Assert.True(_helpersManagementService.CreateHelper(helper, _token.Jwt).Result);
            Assert.True(_helpersManagementService.DeleteHelper(helper.Id, _token.Jwt).Result);
            Assert.NotNull(_helpersManagementService.GetHelper(helper.Id, _token.Jwt).Result);
            Assert.True(_helpersManagementService.UpdateHelper(helper, _token.Jwt).Result);
        }

        [Fact]
        public async void CommentsFunctinalityTest()
        {
            _token = await _accountManagementService.SignIn(TestingObjectProvider.Instance.Login);
            Assert.NotNull(_token);

            var comment = TestingObjectProvider.Instance.Comment;

            Assert.NotNull(_commentsManagementService.GetAllComments(_token.Jwt).Result); 
            Assert.True(_commentsManagementService.DeleteComment(comment.Id, _token.Jwt).Result);
            Assert.NotNull(_helpersManagementService.GetHelper(comment.Id, _token.Jwt).Result);
        }

        public void Dispose()
        {
            _token = null;
        }
    }
}
