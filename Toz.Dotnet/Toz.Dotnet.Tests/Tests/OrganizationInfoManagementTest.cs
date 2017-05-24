using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Organization;
using Toz.Dotnet.Tests.Helpers;
using Xunit;

namespace Toz.Dotnet.Tests.Tests
{
    public class OrganizationInfoManagementTest
    {
        private readonly IOrganizationManagementService _organizationInfoManagementService;
        private readonly Organization _testingOrganization;

        public OrganizationInfoManagementTest()
        {         
            _organizationInfoManagementService = ServiceProvider.Instance.Resolve<IOrganizationManagementService>();
            _organizationInfoManagementService.RequestUri = RequestUriHelper.OrganizationInfoUri;
            _testingOrganization = TestingObjectProvider.Instance.Organization;
        }

        [Fact]
        public void TestDependencyInjectionFromOrganizationInfoManagementService()
        {
            Assert.NotNull(_organizationInfoManagementService);
        }

        [Fact]
        public async void TestOfGettingOrganizationInfo()
        {
            Assert.NotNull(await _organizationInfoManagementService.GetOrganizationInfo());
        }

        [Fact]
        public void TestValidationWithProperData()
        {
            var context = new ValidationContext(_testingOrganization, null, null);
            var result = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(_testingOrganization, context, result, true);

            Assert.True(isValid);
        }

        [Fact]
        public async void TestOfUpdatingOrganizationInfo()
        {
            Assert.True(await _organizationInfoManagementService.UpdateOrCreateInfo(_testingOrganization));
        }

        [Fact]
        public void TestOfUpdatingOrganizationWithNullValue()
        {
            var exception = Record.Exception(() => _organizationInfoManagementService.UpdateOrCreateInfo(null).Result);
            Assert.IsType(typeof(NullReferenceException), exception?.InnerException);
        }

        [Fact]
        public void TestOfGettingOrganizationUsingWrongUrl()
        {
            _organizationInfoManagementService.RequestUri = RequestUriHelper.WrongUrl;
            Assert.Null(_organizationInfoManagementService.GetOrganizationInfo().Result);
            _organizationInfoManagementService.RequestUri = RequestUriHelper.PetsUri;
        }

        private Organization CloneOrganization(Organization copy)
        {
            var clone = new Organization()
            {
                Contact = new Contact()
                {
                    Email = copy.Contact.Email,
                    Website = copy.Contact.Website,
                    Fax = copy.Contact.Fax,
                    Phone = copy.Contact.Phone
                },
                Address = new Address()
                {
                    HouseNumber = copy.Address.HouseNumber,
                    Country = copy.Address.Country,
                    Street = copy.Address.Street,
                    City = copy.Address.City,
                    PostCode = copy.Address.PostCode,
                    ApartmentNumber = copy.Address.ApartmentNumber
                },
                BankAccount = new BankAccount()
                {
                    Number = copy.BankAccount.Number,
                    BankName = copy.BankAccount.BankName
                },
                Name = copy.Name
            };
            return clone;
        }    
     
    }
}
