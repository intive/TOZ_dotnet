using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class HotToHelpInfoManagementTests
    {
        private readonly IHowToHelpInformationService _howToHelpInformationService;
        private readonly HowToHelpInfo _testingObject;
        private readonly JwtToken _token;

        public HotToHelpInfoManagementTests()
        {
            _howToHelpInformationService = ServiceProvider.Instance.Resolve<IHowToHelpInformationService>();
            _howToHelpInformationService.BecomeVolunteerUrl = RequestUriHelper.OrganizationInfoUri;
            _howToHelpInformationService.DonateInfoUrl = RequestUriHelper.HowToHelpUri;
            _testingObject = TestingObjectProvider.Instance.HowToHelpInfo;
            _token = TestingObjectProvider.Instance.JwtToken;
        }

        [Fact]
        public void TestDependencyInjectionFromOrganizationInfoManagementService()
        {
            Assert.NotNull(_howToHelpInformationService);
        }

        [Fact]
        public async void TestOfGettingBecomeVolunteerInfo()
        {
            Assert.NotNull(await _howToHelpInformationService.GetHelpInfo(HowToHelpInfoType.BecomeVolunteer, _token.Jwt));
        }

        [Fact]
        public async void TestOfGettingDonateInfo()
        {
            Assert.NotNull(await _howToHelpInformationService.GetHelpInfo(HowToHelpInfoType.Donate, _token.Jwt));
        }

        [Fact]
        public void TestValidationWithProperData()
        {
            var context = new ValidationContext(_testingObject, null, null);
            var result = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(_testingObject, context, result, true);

            Assert.True(isValid);
        }

        [Fact]
        public async void TestOfUpdatingBecomeVolunteerInfo()
        {
            Assert.True(await _howToHelpInformationService.UpdateOrCreateHelpInfo(_testingObject, HowToHelpInfoType.BecomeVolunteer, _token.Jwt));
        }

        [Fact]
        public async void TestOfUpdatingDonateInfo()
        {
            Assert.True(await _howToHelpInformationService.UpdateOrCreateHelpInfo(_testingObject, HowToHelpInfoType.Donate, _token.Jwt));
        }

        [Fact]
        public void TestOfGettingOrganizationUsingWrongUrl()
        {
            _howToHelpInformationService.BecomeVolunteerUrl = RequestUriHelper.WrongUrl;
            _howToHelpInformationService.DonateInfoUrl = RequestUriHelper.WrongUrl;
            Assert.Null(_howToHelpInformationService.GetHelpInfo(HowToHelpInfoType.BecomeVolunteer, _token.Jwt).Result);

            _howToHelpInformationService.BecomeVolunteerUrl = RequestUriHelper.HowToHelpUri;
            _howToHelpInformationService.DonateInfoUrl = RequestUriHelper.HowToHelpUri;
        }

        public void TestPetValidationIfLengthIsNotValid(string propertyName)
        {
            const int maxLength = 100;

            var info = TestingObjectProvider.Instance.DoShallowCopy(_testingObject);
            info.Description = new string('a',maxLength + 1);

            var context = new ValidationContext(info, null, null);
            var result = new List<ValidationResult>();
            bool validMax = Validator.TryValidateObject(info, context, result, true);
            Assert.False(validMax);
        }
    }
}
