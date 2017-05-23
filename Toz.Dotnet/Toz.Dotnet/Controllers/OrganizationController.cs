using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using System.Threading;
using Toz.Dotnet.Models.Organization;
using System;
using Microsoft.AspNetCore.Routing;

namespace Toz.Dotnet.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationManagementService _organizationManagementService;
        private readonly IBackendErrorsService _backendErrorsService;
        private readonly IStringLocalizer<OrganizationController> _localizer;
        private readonly AppSettings _appSettings;

        public OrganizationController(IOrganizationManagementService organizationManagementService, IStringLocalizer<OrganizationController> localizer,
            IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService)
        {
            _organizationManagementService = organizationManagementService;
            _localizer = localizer;
            _appSettings = appSettings.Value;
            _backendErrorsService = backendErrorsService;
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

                var overallError = _backendErrorsService.UpdateModelState(ModelState);
                if (!string.IsNullOrEmpty(overallError))
                {
                    this.ViewData["UnhandledError"] = overallError;
                }
            }

            ViewData["EditMode"] = true;
            return View("Info", organization);  
        }
    }
}
