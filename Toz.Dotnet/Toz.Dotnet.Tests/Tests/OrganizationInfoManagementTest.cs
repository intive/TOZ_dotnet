using System;
using System.Collections.Generic;
using System.Text;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.OrganizationSubtypes;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class OrganizationInfoManagementTest
    {
        private readonly IOrganizationManagementService _organizationInfoManagementService;
        private readonly IAuthService _authService;

        public OrganizationInfoManagementTest()
        {
            _organizationInfoManagementService = ServiceProvider.Instance.Resolve<IOrganizationManagementService>();
            _authService = ServiceProvider.Instance.Resolve<IAuthService>();
            _organizationInfoManagementService.RequestUri = RequestUriHelper.OrganizationInfoUri;
            _authService.RequestUri = RequestUriHelper.JwtTokenUrl;
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
            // --> TEMPORARY
            for (int i = 0; i < 5; i++)
            {
                await _authService.SignIn(new Models.Login() { Email = $"TOZ_user{i}.email@gmail.com", Password = $"TOZ_name_{i}" });
                if (_authService.IsAuth)
                {
                    break;
                }
            }
            Assert.True(_authService.IsAuth);
            _authService.SighOut();
        }

        [Fact]
        public async void TestOfUpdatingOrganizationInfo()
        {
            
            var originalOrganizationInfo = _organizationInfoManagementService.GetOrganizationInfo().Result;

            var customOrganizationInfo = new Organization()
            {
                Name = "Test",
                Address = new Address()
                {
                    ApartmentNumber = "45",
                    City = "TestCity",
                    Country = "TestCountry",
                    HouseNumber = "11",
                    PostCode = "73-220",
                    Street = "TestStreet"
                },
                BankAccount = new BankAccount()
                {
                    BankName = "TestBankName",
                    Number = "61109010140000071219812874"
                },
                Contact = new Contact()
                {
                    Email = "testEmail@test.com",
                    Fax = "123456789",
                    Phone = "123456789",
                    Website = "http://testwebsite.com"
                }
            };
            // --> TEMPORARY
            for (int i = 0; i < 5; i++)
            {
                await _authService.SignIn(new Models.Login() { Email = $"TOZ_user{i}.email@gmail.com", Password = $"TOZ_name_{i}" });
                if (_authService.IsAuth)
                {
                    break;
                }
            }
            if (_authService.IsAuth)
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
