using System;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Toz.Dotnet.Resources.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Net.Http;

namespace Toz.Dotnet.Controllers
{
    public class PetsController : TozControllerBase<PetsController>
    {
        private readonly IFilesManagementService _filesManagementService;
        private readonly IPetsManagementService _petsManagementService;
        private readonly IOptions<AppSettings> _appSettings;

        public PetsController(IFilesManagementService filesManagementService, IPetsManagementService petsManagementService,
            IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService) : base(backendErrorsService, localizer, appSettings)
        {
            _filesManagementService = filesManagementService;
            _petsManagementService = petsManagementService;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Pet> pets = await _petsManagementService.GetAllPets(cancellationToken);
            foreach (var pet in pets)
            {
                if (!string.IsNullOrEmpty(pet.ImageUrl))
                {
                    try
                    {
                        var downloadedImg = _filesManagementService.DownloadImage(_appSettings.Value.BaseUrl + pet.ImageUrl);

                        if (downloadedImg != null)
                        {
                            var thumbnail = _filesManagementService.GetThumbnail(downloadedImg);
                            pet.Photo = _filesManagementService.ImageToByteArray(thumbnail);
                        }
                    }
                    catch (HttpRequestException)
                    {
                        pet.Photo = null;
                    }
                    catch (AggregateException)
                    {
                        pet.Photo = null;
                    }
                }
            }
            return View(pets.OrderByDescending(x => x.Created).ThenByDescending(x => x.LastModified).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("Name, Type, Sex, Description, Address")]
            Pet pet, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (await _petsManagementService.CreatePet(pet, cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(pet);
            }

            return PartialView(pet);

        }

        public IActionResult Add()
        {
            return PartialView(new Pet());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id, Name, Type, Sex, Description, Address, AddingTime")]
            Pet pet, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (await _petsManagementService.UpdatePet(pet))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(pet);
            }

            return PartialView(pet);
        }

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            return PartialView("Edit", await _petsManagementService.GetPet(id));
        }

        public async Task<ActionResult> Images(string id, CancellationToken cancellationToken)
        {
            return PartialView("Images", await _petsManagementService.GetPet(id));
        }


        /*        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
                {
                    var pet = await _petsManagementService.GetPet(id);
                    if(pet != null)
                    {
                        await _petsManagementService.DeletePet(pet);
                    }

                    return RedirectToAction("Index");
                }*/
    }

}