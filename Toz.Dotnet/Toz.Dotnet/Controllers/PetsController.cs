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
using Toz.Dotnet.Authorization;
using Toz.Dotnet.Models.Images;
using Toz.Dotnet.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Toz.Dotnet.Controllers
{
    public class PetsController : TozControllerBase<PetsController>
    {
        private readonly IFilesManagementService _filesManagementService;
        private readonly IPetsManagementService _petsManagementService;
        private readonly IPetsStatusManagementService _petsStatusManagementService;
        private readonly IHelpersManagementService _helpersManagementService;

        private readonly IOptions<AppSettings> _appSettings;

        public PetsController(IFilesManagementService filesManagementService, IPetsManagementService petsManagementService,
            IPetsStatusManagementService petsStatusManagementService, IHelpersManagementService helpersManagementService, IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings,
            IBackendErrorsService backendErrorsService, IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _filesManagementService = filesManagementService;
            _petsManagementService = petsManagementService;
            _petsStatusManagementService = petsStatusManagementService;
            _helpersManagementService = helpersManagementService;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Pet> pets = await _petsManagementService.GetAllPets(CurrentCookiesToken, cancellationToken);
            List<PetViewModel> viewModel = new List<PetViewModel>();

            if (pets == null)
            {
                return View();
            }

            foreach (var pet in pets)
            {
                viewModel.Add(new PetViewModel
                {
                    ThePet = pet,
                    ThePetStatus = string.IsNullOrEmpty(pet.PetsStatus)
                        ? new PetsStatus { Name = StringLocalizer["Lack"] }
                        : await _petsStatusManagementService.GetStatus(pet.PetsStatus, CurrentCookiesToken, cancellationToken),
                    TheHelper = string.IsNullOrEmpty(pet.HelperId)
                        ? new Helper { Address = "TOZ" }
                        : await _helpersManagementService.GetHelper(pet.HelperId, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken)
                });
            
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
            return View(viewModel.OrderByDescending(x => x.ThePet.Created).ThenByDescending(x => x.ThePet.LastModified));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PetViewModel pet, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (await _petsManagementService.CreatePet(pet.ThePet, CurrentCookiesToken, cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(pet);
            }

            return PartialView(pet);

        }

        public async Task<IActionResult> Add(CancellationToken cancellationToken)
        {
            List<PetsStatus> petsStatus = (await _petsStatusManagementService.GetAllStatus(CurrentCookiesToken, cancellationToken))
                 .OrderBy(s => s.Name)
                 .ToList();

            PetViewModel pet = new PetViewModel
            {
                ThePet = new Pet(),
                TheStatusList = new SelectList(petsStatus, "Id", "Name")
            };

            return PartialView(pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PetViewModel pet, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (await _petsManagementService.UpdatePet(pet.ThePet, CurrentCookiesToken, cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(pet);
            }

            return PartialView(pet);
        }

        [HttpPost]
        public async Task<IActionResult> Avatar(string id, CancellationToken cancellationToken)
        {
            var files = Request.Form.Files;
            if (await _filesManagementService.UploadPetAvatar(id, CurrentCookiesToken, files, cancellationToken))
            {
                return Json(new { success = true });
            }

            CheckUnexpectedErrors();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Gallery(string id, CancellationToken cancellationToken)
        {
            Pet pet = await _petsManagementService.GetPet(id, CurrentCookiesToken, cancellationToken);
            List<Photo> gallery = pet.Gallery;
            List<FineUploader> json = new List<FineUploader>();

            for(int i = 0; i < gallery.Count; i++)
            {
                var photo = gallery[i];
                json.Add(new FineUploader { UUID = photo.Id, Name = $"Zdj�cie {i+1}.jpg", ThumbnailUrl = $"{_appSettings.Value.ThumbnailsBaseUrl}{photo.FileUrl}" });
            }

            CheckUnexpectedErrors();
            return Json(json);
        }

        [HttpPost]
        public async Task<IActionResult> Gallery(string id, string imageId, CancellationToken cancellationToken)
        {
            var files = Request.Form.Files;
            if (await _filesManagementService.UploadPetGalleryImage(id, CurrentCookiesToken, files, cancellationToken))
            {
                return Json(new { success = true });
            }

            CheckUnexpectedErrors();
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> GalleryDelete(string id, CancellationToken cancellationToken)
        {
            var imageId = Request.Form["qquuid"].ToString();
            if (await _filesManagementService.DeletePetGalleryImage(id, imageId, CurrentCookiesToken, cancellationToken))
            {
                return Json(new { success = true });
            }

            CheckUnexpectedErrors();
            return Json(new { success = false });
        }

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            List<PetsStatus> petsStatus = (await _petsStatusManagementService.GetAllStatus(CurrentCookiesToken, cancellationToken))
                 .OrderBy(s => s.Name)
                 .ToList();

            PetViewModel pet = new PetViewModel
            {
                ThePet = await _petsManagementService.GetPet(id, CurrentCookiesToken, cancellationToken),
                TheStatusList = new SelectList(petsStatus, "Id", "Name")
            };

            return PartialView("Edit", pet);
        }

        public async Task<ActionResult> Images(string id, CancellationToken cancellationToken)
        {
            return PartialView("Images", await _petsManagementService.GetPet(id, CurrentCookiesToken, cancellationToken));
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