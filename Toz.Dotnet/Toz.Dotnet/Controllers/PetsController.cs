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
    public class PetsController : Controller
    {
        private IFilesManagementService _filesManagementService;
        private IPetsManagementService _petsManagementService;
		private readonly IStringLocalizer<PetsController> _localizer;
        private readonly AppSettings _appSettings;
        private static byte[] _lastAcceptPhoto;
        private string _validationPhotoAlert;
		
        public PetsController(IFilesManagementService filesManagementService, IPetsManagementService petsManagementService, IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings)
        {
            _filesManagementService = filesManagementService;
            _petsManagementService = petsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Pet> pets = await _petsManagementService.GetAllPets();
            //todo add photo if will be avaialbe on backends
            var img = _filesManagementService.DownloadImage("http://i.pinger.pl/pgr167/7dc36d63001e9eeb4f01daf3/kot%20ze%20shreka9.jpg");
            var thumbnail = _filesManagementService.GetThumbnail2(img);
            pets.ForEach(pet => pet.Photo = _filesManagementService.ImageToByteArray(thumbnail)); // temporary
            return View(pets.OrderByDescending(x => x.AddingTime).ThenByDescending(x => x.LastEditTime).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("Name, Type, Sex, Description, Address")] 
            Pet pet, [Bind("Photo")] IFormFile photo, CancellationToken cancellationToken)
        {
            bool result = ValidatePhoto(pet, photo);
            pet.ImageUrl = "storage/a5/0d/4d/a50d4d4c-ccd2-4747-8dec-d6d7f521336e.jpg"; //temporary
            
            if (pet != null && result && ModelState.IsValid)
            {
                    if (await _petsManagementService.CreatePet(pet))
                    {
                        _lastAcceptPhoto = null;
                        _validationPhotoAlert = null;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return BadRequest();
                    }
            }
            else
            {
                if(!result)
                {
                    ViewData["ValidationPhotoAlert"] = _validationPhotoAlert;
                    if(_lastAcceptPhoto != null)
                    {
                        pet.Photo = _lastAcceptPhoto;
                        ViewData["SelectedPhoto"] = "PhotoAlertWithLastPhoto";
                    }
                    else
                    {
                        ViewData["SelectedPhoto"] = "PhotoAlertWithoutPhoto";
                    }
                }
                return View(pet);
            }
        } 

        public IActionResult Add()
        {
            return View(new Pet());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id, Name, Type, Sex, Description, Address, AddingTime")] 
            Pet pet, [Bind("Photo")] IFormFile photo, CancellationToken cancellationToken)
        {
            //todo add photo if will be available on backends
            _lastAcceptPhoto = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; //get photo from backend, if available
            pet.ImageUrl = "storage/a5/0d/4d/a50d4d4c-ccd2-4747-8dec-d6d7f521336e.jpg"; //temporary

            bool result = ValidatePhoto(pet, photo);

            if (pet != null && result && ModelState.IsValid)
            {
                    if (await _petsManagementService.UpdatePet(pet))
                    {
                        _lastAcceptPhoto = null;
                        _validationPhotoAlert = null;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return BadRequest();
                    }
            }
            else
            {
                if(!result)
                {
                    ViewData["ValidationPhotoAlert"] = _validationPhotoAlert;
                    if(_lastAcceptPhoto != null)
                    {
                        pet.Photo = _lastAcceptPhoto;
                        ViewData["SelectedPhoto"] = "PhotoAlertWithLastPhoto";
                    }
                    else
                    {
                        ViewData["SelectedPhoto"] = "PhotoAlertWithoutPhoto";
                    }
                }
                return View(pet);
            }
            
        } 

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken) 
        {
            return View(await _petsManagementService.GetPet(id));
        }

        
        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var pet = await _petsManagementService.GetPet(id);
            if(pet != null)
            {
                await _petsManagementService.DeletePet(pet);
            }

            return RedirectToAction("Index");
        }

        private bool IsAcceptPhotoType(string photoType, string[] acceptTypes)
        {
            foreach(var type in acceptTypes)
            {
                if(type == photoType)
                    return true;
            }
            return false;
        }

        private bool ValidatePhoto(Pet pet, IFormFile photo)
        {
            if(photo != null)
            {
                if(IsAcceptPhotoType(photo.ContentType, _appSettings.AcceptPhotoTypes))
                {
                    if(photo.Length > 0)
                    {
                        pet.Photo = _petsManagementService.ConvertPhotoToByteArray(photo.OpenReadStream());
                        _lastAcceptPhoto = pet.Photo;
                        return true;
                    }
                    else
                    {
                        _validationPhotoAlert = "EmptyFile";
                        return false;
                    }
                }
                else
                {
                    _validationPhotoAlert = "WrongFileType";
                    return false; 
                }
            }
            else
            {
                if(_lastAcceptPhoto != null)
                {
                    pet.Photo = _lastAcceptPhoto;
                }
                return true;
            }
        }
    }
	
}