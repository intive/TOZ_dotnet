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

namespace Toz.Dotnet.Controllers
{
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
		private readonly IStringLocalizer<PetsController> _localizer;
        private readonly AppSettings _appSettings;
        private static byte[] _lastAcceptPhoto;
        private string _validationPhotoAlert;
		
        public PetsController(IPetsManagementService petsManagementService, IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings)
        {
            _petsManagementService = petsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Pet> pets = await _petsManagementService.GetAllPets();
            //todo add photo if will be avaialbe on backends
            pets.ForEach(pet=> pet.Photo = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }); // temporary
            return View(pets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("Name, Type, Sex, Description, Address")] 
            Pet pet, [Bind("Photo")] IFormFile photo, CancellationToken cancellationToken)
        {
            bool result = ValidatePhoto(pet, photo);
            
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