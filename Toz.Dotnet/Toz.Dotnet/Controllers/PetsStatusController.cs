using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Toz.Dotnet.Authorization;

namespace Toz.Dotnet.Controllers
{
    public class PetsStatusController : TozControllerBase<PetsStatusController>
    {
        private readonly IPetsStatusManagementService _petsStatusManagementService;
        private readonly IOptions<AppSettings> _appSettings;

        public PetsStatusController(IPetsStatusManagementService petsStatusManagementService,
            IStringLocalizer<PetsStatusController> localizer, IOptions<AppSettings> appSettings,
            IBackendErrorsService backendErrorsService, IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _petsStatusManagementService = petsStatusManagementService;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<PetsStatus> petsStatus = await _petsStatusManagementService.GetAllStatus(CurrentCookiesToken, cancellationToken);
            return View(petsStatus.OrderByDescending(x => x.Name).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PetsStatus petsStatus, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (await _petsStatusManagementService.CreateStatus(petsStatus, CurrentCookiesToken, cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(petsStatus);
            }

            return PartialView(petsStatus);

        }

        public IActionResult Add()
        {
            return PartialView(new PetsStatus());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PetsStatus petsStatus, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (await _petsStatusManagementService.UpdateStatus(petsStatus, CurrentCookiesToken, cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(petsStatus);
            }

            return PartialView(petsStatus);
        }

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            return PartialView("Edit", await _petsStatusManagementService.GetStatus(id, CurrentCookiesToken, cancellationToken));
        }

        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            return PartialView("Delete", await _petsStatusManagementService.GetStatus(id, CurrentCookiesToken, cancellationToken));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(PetsStatus petsStatus, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (await _petsStatusManagementService.DeleteStatus(petsStatus, CurrentCookiesToken, cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(petsStatus);
            }

            return PartialView(petsStatus);
        }
    }
}