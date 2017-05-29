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

namespace Toz.Dotnet.Controllers
{
    public class PetsController : TozControllerBase<PetsController>
    {
        private readonly IFilesManagementService _filesManagementService;
        private readonly IPetsManagementService _petsManagementService;
        private readonly IHelpersManagementService _helpersManagementService;
        private readonly IOptions<AppSettings> _appSettings;

        public PetsController(IFilesManagementService filesManagementService, IPetsManagementService petsManagementService,
            IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings, IHelpersManagementService helpersManagementService,
            IBackendErrorsService backendErrorsService, IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _filesManagementService = filesManagementService;
            _petsManagementService = petsManagementService;
            _helpersManagementService = helpersManagementService;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Pet> pets = await _petsManagementService.GetAllPets(CurrentCookiesToken, cancellationToken);
            List<PetViewModel> viewModel = new List<PetViewModel>();
            foreach (var pet in pets)
            {
                viewModel.Add(new PetViewModel
                {
                    ThePet = pet,
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
                if (await _petsManagementService.CreatePet(pet, CurrentCookiesToken, cancellationToken))
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
                if (await _petsManagementService.UpdatePet(pet, CurrentCookiesToken, cancellationToken))
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

            foreach (var photo in gallery)
            {
                json.Add(new FineUploader { UUID = photo.Id, Name = $"{photo.Id}.jpg", ThumbnailUrl = $"{_appSettings.Value.BaseUrl}{photo.FileUrl}" });
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
            return PartialView("Edit", await _petsManagementService.GetPet(id, CurrentCookiesToken,cancellationToken));
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