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
        private readonly IOrganizationInfoManagementService _organizationInfoManagementService;

        public OrganizationInfoManagementTest()
        {
            _organizationInfoManagementService = ServiceProvider.Instance.Resolve<IOrganizationInfoManagementService>();
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
        public void TestOfUpdatingOrganizationInfo()
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
                    Number = "1235121231231233123"
                },
                Contact = new Contact()
                {
                    Email = "testEmail@test.com",
                    Fax = "1234",
                    Phone = "123412341",
                    Website = "testwebsite.com"
                }
            };

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
