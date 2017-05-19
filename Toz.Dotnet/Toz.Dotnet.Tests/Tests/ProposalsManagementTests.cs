using System;
using System.Collections.Generic;
using System.Text;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class ProposalsManagementTests
    {
        private readonly Proposal _testingProposal;
        private readonly IProposalsManagementService _proposalsManagementService;

        public ProposalsManagementTests()
        {
            _proposalsManagementService = ServiceProvider.Instance.Resolve<IProposalsManagementService>();
            _proposalsManagementService.RequestUri = RequestUriHelper.ProposalsUri;
            _testingProposal = TestingObjectProvider.Instance.Proposal;
        }

        [Fact]
        public void TestDependencyInjection()
        {
            Assert.NotNull(_proposalsManagementService);
        }

        [Fact]
        public void TestRequestedUriNotEmpty()
        {
            Assert.True(!string.IsNullOrEmpty(_proposalsManagementService.RequestUri));
        }

        [Fact]
        public void TestOfGettingAllProposals()
        {
            Assert.NotNull(_proposalsManagementService.GetAllProposals().Result);
        }

        [Fact]
        public void TestOfCreatingNewProposal()
        {
            var result = _proposalsManagementService.CreateProposal(_testingProposal).Result;
            Assert.True(result);
        }

        [Fact]
        public void TestOfDeletingSpecifiedProposal()
        {
            Assert.True(_proposalsManagementService.DeleteProposal(_testingProposal).Result);
        }

        [Fact]
        public void TestOfGettingSpecifiedProposal()
        {
            Assert.NotNull(_proposalsManagementService.GetProposal(_testingProposal.Id).Result);
        }

        [Fact]
        public void TestOfUpdatingProposal()
        {
            Assert.True(_proposalsManagementService.UpdateProposal(_testingProposal).Result);
        }

        [Fact]
        public void TestOfUpdatingProposalWithNullValue()
        {
            var exception = Record.Exception(() => _proposalsManagementService.UpdateProposal(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfDeletingProposalThatIsNull()
        {
            var exception = Record.Exception(() => _proposalsManagementService.DeleteProposal(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingAllProposalsUsingWrongUrl()
        {
            _proposalsManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_proposalsManagementService.GetAllProposals().Result);
            _proposalsManagementService.RequestUri = RequestUriHelper.PetsUri;
        }
    }
}
