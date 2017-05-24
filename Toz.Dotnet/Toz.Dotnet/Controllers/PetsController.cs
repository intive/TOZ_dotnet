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

namespace Toz.Dotnet.Controllers
{
    public class PetsController : TozControllerBase<PetsController>
    {
        private readonly IFilesManagementService _filesManagementService;
        private readonly IPetsManagementService _petsManagementService;
		
        public PetsController(IFilesManagementService filesManagementService, IPetsManagementService petsManagementService,
            IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService) : base(backendErrorsService, localizer, appSettings)
        {
            _filesManagementService = filesManagementService;
            _petsManagementService = petsManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Pet> pets = await _petsManagementService.GetAllPets(cancellationToken);
            //todo add photo if will be avaialbe on backends
            var img = _filesManagementService.DownloadImage(@"http://i.pinger.pl/pgr167/7dc36d63001e9eeb4f01daf3/kot%20ze%20shreka9.jpg");
            var thumbnail = _filesManagementService.GetThumbnail(img);
            pets.ForEach(pet => pet.Photo = _filesManagementService.ImageToByteArray(thumbnail)); // temporary
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