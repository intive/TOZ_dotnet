using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
		private readonly IStringLocalizer<PetsController> _localizer;
        private readonly AppSettings _appSettings;
        private static byte[] _lastAcceptPhoto;
        private static bool _firstNoAcceptPhoto = false;
        private string _validationPhotoAlert;
		
        public PetsController(IPetsManagementService petsManagementService, IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings)
        {
            _petsManagementService = petsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            List<Pet> pets = _petsManagementService.GetAllPets().Result;
            //todo add photo if will be avaialbe on backends
            pets.ForEach(pet=> pet.Photo = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }); // temporary
            return View(pets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(
            [Bind("Name, Type, Sex, Description, Address")] 
            Pet pet, [Bind("Photo")] IFormFile photo)
        {
            bool result = ValidatePhoto(pet, photo);
            if (pet != null && result && ModelState.IsValid)
            {
                    if (_petsManagementService.CreatePet(pet).Result)
                    {
                        _lastAcceptPhoto = null;
                        _validationPhotoAlert = null;
                        _firstNoAcceptPhoto = false;
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
        public IActionResult Edit(
            [Bind("Id, Name, Type, Sex, Description, Address, AddingTime")] 
            Pet pet, [Bind("Photo")] IFormFile photo)
        {
            //todo add photo if will be available on backends
            _lastAcceptPhoto = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; //get photo from backend, if available
            bool result = ValidatePhoto(pet, photo);
            if (pet != null && result && ModelState.IsValid)
            {
                    if (_petsManagementService.UpdatePet(pet).Result)
                    {
                        _lastAcceptPhoto = null;
                        _validationPhotoAlert = null;
                        _firstNoAcceptPhoto = false;
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

        public ActionResult Edit(string id) 
        {
            return View(_petsManagementService.GetPet(id).Result);
        }

        public ActionResult Delete(string id)
        {
            var pet = _petsManagementService.GetPet(id).Result;
            if(pet != null)
            {
                _petsManagementService.DeletePet(pet).Wait();
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
                _firstNoAcceptPhoto = false;
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
                        _firstNoAcceptPhoto = true;
                        _validationPhotoAlert = "EmptyFile";
                        return false;
                    }
                }
                else
                {
                    _firstNoAcceptPhoto = true;
                    _validationPhotoAlert = "WrongFileType";
                    return false; 
                }
            }
            else
            {
                if(_lastAcceptPhoto != null)
                {
                    pet.Photo = _lastAcceptPhoto;
                    return true;
                }
                if(_firstNoAcceptPhoto)
                {
                    return true;
                }
                _validationPhotoAlert = "NoFileSelected";
                _firstNoAcceptPhoto = true;
                return false;
            }
        }
    }
	
}
