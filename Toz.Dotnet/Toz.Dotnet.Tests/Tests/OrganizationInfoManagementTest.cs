using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.OrganizationSubtypes;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class OrganizationInfoManagementTest
    {
        private readonly AuthService _authHelper;
        private readonly IOrganizationManagementService _organizationInfoManagementService;

        public OrganizationInfoManagementTest()
        {
            _authHelper = new AuthService();
            _organizationInfoManagementService = ServiceProvider.Instance.Resolve<IOrganizationManagementService>();
            _organizationInfoManagementService.RequestUri = RequestUriHelper.OrganizationInfoUri;
        }

        [Fact]
        public void TestDependencyInjectionFromOrganizationInfoManagementService()
        {
            Assert.NotNull(_organizationInfoManagementService);
        }

        [Fact]
        public void TestOfGettingOrganizationInfo()
        {
            var organizationInfo = _organizationInfoManagementService.GetOrganizationInfo().Result;

            if (organizationInfo != null)
            {
                Assert.NotNull(organizationInfo.BankAccount);
                Assert.NotNull(organizationInfo.Contact);
            }
        }

        [Fact]
        public async void TestAuthentication()
        {
            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            Assert.True(_authHelper.AuthHelper.IsAuth);
            _authHelper.AuthHelper.SighOut();
        }

        [Fact]
        public async void TestOfUpdatingOrganizationInfo()
        {
            var originalOrganizationInfo = _organizationInfoManagementService.GetOrganizationInfo().Result;

            var customOrganizationInfo = new Organization
            {
                Name = "Test",
                Address = new Address
                {
                    ApartmentNumber = "45",
                    City = "TestCity",
                    Country = "TestCountry",
                    HouseNumber = "11",
                    PostCode = "73-220",
                    Street = "TestStreet"
                },

                BankAccount = new BankAccount
                {
                    BankName = "TestBankName",
                    Number = "61109010140000071219812874"
                },

                Contact = new Contact
                {
                    Email = "testEmail@test.com",
                    Fax = "123456789",
                    Phone = "123456789",
                    Website = "http://testwebsite.com"
                }
            };

            if (!_authHelper.AuthHelper.IsAuth)
            {
                Assert.True(await _authHelper.SignIn());
            }
            else
            {
                var firstUpdateResult = _organizationInfoManagementService.UpdateOrCreateInfo(customOrganizationInfo).Result;
                Assert.True(firstUpdateResult);

                if (originalOrganizationInfo != null)
                {
                    var secondUpdateResult = _organizationInfoManagementService.UpdateOrCreateInfo(originalOrganizationInfo).Result;
                    Assert.True(secondUpdateResult);
                }
            }

            
        }
    }
}
