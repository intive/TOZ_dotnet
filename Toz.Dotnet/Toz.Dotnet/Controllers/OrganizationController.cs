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

        public OrganizationController(IOrganizationManagementService organizationManagementService, IStringLocalizer<OrganizationController> localizer, IOptions<AppSettings> appSettings)
        {
            _organizationManagementService = organizationManagementService;
            _localizer = localizer;
            _appSettings = appSettings.Value;
        }


        [HttpGet]
        public async Task<IActionResult> Info(bool edit, Organization organizationInstance = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            ViewData["EditMode"] = edit;

            var organizationInfo = await _organizationManagementService.GetOrganizationInfo(cancellationToken);
            if (organizationInfo != null)
            {
                return View(organizationInfo);
            }
            ViewData["EditMode"] = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(Organization organization, CancellationToken cancellationToken)
        {
            if (organization != null && ModelState.IsValid)
            {
                organization.Contact.Website = new UriBuilder(organization.Contact.Website).Uri.ToString();
                if (await _organizationManagementService.UpdateOrCreateInfo(organization, cancellationToken))
                {
                    return RedirectToAction("Info", new RouteValueDictionary(new { edit = false }));
                }
                return BadRequest();       
            }

            ViewData["EditMode"] = true;
            return View("Info", organization);  
        }
    }
}
