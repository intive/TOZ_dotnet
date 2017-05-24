using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Routing;
using Toz.Dotnet.Models.Organization;

namespace Toz.Dotnet.Controllers
{
    public class OrganizationController : TozControllerBase<OrganizationController>
    {
        private readonly IOrganizationManagementService _organizationManagementService;

        public OrganizationController(IOrganizationManagementService organizationManagementService, IStringLocalizer<OrganizationController> localizer,
            IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService) : base(backendErrorsService, localizer, appSettings)
        {
            _organizationManagementService = organizationManagementService;
        }


        [HttpGet]
        public async Task<IActionResult> Info(bool edit, CancellationToken cancellationToken = default(CancellationToken))
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

                CheckUnexpectedErrors();
            }

            ViewData["EditMode"] = true;
            return View("Info", organization);  
        }
    }
}
