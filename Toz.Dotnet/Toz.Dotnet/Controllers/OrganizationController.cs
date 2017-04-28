using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using System.Threading;
using Toz.Dotnet.Models.OrganizationSubtypes;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Toz.Dotnet.Extensions;

namespace Toz.Dotnet.Controllers
{
    public class OrganizationController : Controller
    {
        private IOrganizationManagementService _organizationManagementService;
        private readonly IStringLocalizer<OrganizationController> _localizer;
        private readonly AppSettings _appSettings;

        //public OrganizationController(IOrganizationManagementService organizationManagementService, IStringLocalizer<OrganizationController> localizer, IOptions<AppSettings> appSettings)
        //{
        //    _organizationManagementService = organizationManagementService;
        //    _localizer = localizer;
        //    _appSettings = appSettings.Value;
        //}

        public OrganizationController(IStringLocalizer<OrganizationController> localizer, IOptions<AppSettings> appSettings)
        {
            //_organizationManagementService = organizationManagemntService;
            _localizer = localizer;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult Info(bool edit, Organization organizationInstance = null)
        {
            ViewData["EditMode"] = edit;

            Address a = new Address
            {
                Street = "Testowa",
                HouseNumber = "25",
                ApartmentNumber = "10",
                PostCode = "71-353",
                City = "Szczecin",
                Country = "Polska"
            };

            Contact c = new Contact
            {
                Email = "toz@intive.com",
                Phone = "123456789",
                Fax = "123456789",
                Website = "http://www.toz-szczecin.pl"
            };

            BankAccount b = new BankAccount
            {
                Number = "12345678901234567890123456",
                BankName = "Bank Opieki nad Zwierzętami SA"
            };

            Organization testOrganization = new Organization
            {
                Name = "Towarzystwo Opieki nad Zwięrzętami",
                Address = a,
                Contact = c,
                BankAccount = b
            };
            return View(testOrganization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(
            Organization organization, CancellationToken cancellationToken)
        {
            if (organization != null && ModelState.IsValid)
            {
                //if(await _organizationManagementService.GetOrganizationInfo() == null)
                //{
                    //if (await _organizationManagementService.CreateOrganizationInfo(organization, cancellationToken))
                    //{
                        return RedirectToAction("Info", new RouteValueDictionary(new { edit = false }));
                   // }
                    return BadRequest();
                //}
                //else
                //{
                    //if (await _organizationManagementService.UpdateOrganizationInfo(organization, cancellationToken))
                   // {
                        return RedirectToAction("Info");
                    //}
                    return BadRequest();
               // }           
            }

            ViewData["EditMode"] = true;
            return View("Info", organization);  
        }
    }
}
