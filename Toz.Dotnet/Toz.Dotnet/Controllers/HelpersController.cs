using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Authorization;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public class HelpersController : TozControllerBase<HelpersController>
    {
        private readonly IHelpersManagementService _helpersManagementService;

        public HelpersController(IHelpersManagementService helpersManagementService,
            IStringLocalizer<HelpersController> localizer, IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService, IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _helpersManagementService = helpersManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Helper> helpers = await _helpersManagementService.GetAllHelpers(AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
            return View(helpers.OrderByDescending(x => x.Created).ThenByDescending(x => x.LastModified).ToList());
        }

        public IActionResult Add()
        {
            return PartialView(new Helper());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("Category, Name, Surname, PhoneNumber, Email, Address, Notes")]
            Helper helper, CancellationToken cancellationToken)
        {
            if (helper != null && ModelState.IsValid)
            {
                if (await _helpersManagementService.CreateHelper(helper, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
            }

            return PartialView(helper);
        }

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            return PartialView("Edit", await _helpersManagementService.GetHelper(id, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id, Category, Name, Surname, PhoneNumber, Email, Address, Notes, Created")]
            Helper helper, CancellationToken cancellationToken)
        {
            if (helper != null && ModelState.IsValid)
            {
                if (await _helpersManagementService.UpdateHelper(helper, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken))
                {
                    return Json(new { success = true });
                }
                
                CheckUnexpectedErrors();
            }

            return PartialView(helper);
        }

        public IActionResult Delete(CancellationToken cancellationToken)
        {
            return PartialView("Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (! string.IsNullOrEmpty(id) && await _helpersManagementService.DeleteHelper(id, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken))
            {
                return Json(new { success = true });
            }

            CheckUnexpectedErrors();
            return PartialView("Delete");
        }
    }
}
