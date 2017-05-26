using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.ViewModels;
using Toz.Dotnet.Resources.Configuration;
using Toz.Dotnet.Authorization;

namespace Toz.Dotnet.Controllers
{
    public class HowToHelpController : TozControllerBase<HowToHelpController>
    {
        private readonly IHowToHelpInformationService _howToHelpInformationService;

        public HowToHelpController(IHowToHelpInformationService howToHelpInformationService, IBackendErrorsService backendErrorsService,
            IStringLocalizer<HowToHelpController> localizer, IOptions<AppSettings> settings, IAuthService authService)
            : base(backendErrorsService, localizer, settings, authService)
        {
            _howToHelpInformationService = howToHelpInformationService;
        }

        public async Task<IActionResult> Info(bool edit, CancellationToken cancellationToken = default(CancellationToken))
        {
            ViewData["EditMode"] = edit;

            var becomeVolunteerInfo = await _howToHelpInformationService.GetHelpInfo(HowToHelpInfoType.BecomeVolunteer, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
            var donateInfo = await _howToHelpInformationService.GetHelpInfo(HowToHelpInfoType.Donate, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
            if (becomeVolunteerInfo != null && donateInfo != null)
            {
                return View(new HowToHelpViewModel()
                {
                    BecomeVolunteerInfo = becomeVolunteerInfo,
                    DonateInfo = donateInfo
                });
            }
            ViewData["EditMode"] = false;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(HowToHelpViewModel howToHelpViewModel, CancellationToken cancellationToken)
        {
            if (howToHelpViewModel != null && ModelState.IsValid)
            {
                var updateVolunteerInfoResult = await _howToHelpInformationService.UpdateOrCreateHelpInfo(
                    howToHelpViewModel.BecomeVolunteerInfo, HowToHelpInfoType.BecomeVolunteer, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);

                var updateDonateInfoResult = await _howToHelpInformationService.UpdateOrCreateHelpInfo(
                    howToHelpViewModel.DonateInfo, HowToHelpInfoType.Donate, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);

                if (updateDonateInfoResult && updateVolunteerInfoResult)
                {
                    return RedirectToAction("Info", new RouteValueDictionary(new { edit = false }));
                }

                CheckUnexpectedErrors();
            }

            ViewData["EditMode"] = true;
            return View("Info", howToHelpViewModel);
        }
    }
}
