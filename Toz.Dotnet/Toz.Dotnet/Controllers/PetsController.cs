using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;
using Toz.Dotnet.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace Toz.Dotnet.Controllers
{
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
		private readonly IStringLocalizer<PetsController> _localizer;
        private readonly AppSettings _appSettings;
        private static byte[] lastAcceptPhoto;
		
        public PetsController(IPetsManagementService petsManagementService, IStringLocalizer<PetsController> localizer, IOptions<AppSettings> appSettings)
        {
            _petsManagementService = petsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            return View(_petsManagementService.GetAllPets());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(
            [Bind("Name, Type, Sex, Description, Address")] 
            Pet pet, [Bind("Photo")] IFormFile photo)
        {
            if(lastAcceptPhoto != null && photo == null)
            {
                pet.Photo = lastAcceptPhoto;
            }
            else
            {
                var result = ValidatePhoto(pet, photo);
                if(result != null)
                {
                    return result;
                }
                else
                {
                    lastAcceptPhoto = _petsManagementService.ConvertPhotoToByteArray(photo.OpenReadStream());
                }
            }

            if (pet != null && ModelState.IsValid)
            {
                if (_petsManagementService.CreatePet(pet)) 
                {
                    lastAcceptPhoto = null;
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                } 
            }
            else
            {
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
            if(lastAcceptPhoto != null)
            {
                pet.Photo = lastAcceptPhoto;
            }
            else
            {
                if(photo != null)
                {
                    var result = ValidatePhoto(pet, photo);
                    if(result != null)
                    {
                        return result;
                    }
                    else
                    {
                        lastAcceptPhoto = _petsManagementService.ConvertPhotoToByteArray(photo.OpenReadStream());
                    }
                }
            }

            if (pet != null && ModelState.IsValid)
            {
                if (_petsManagementService.UpdatePet(pet)) 
                {
                    lastAcceptPhoto = null;
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                } 
            } 
            else
            {
                return View(pet);
            }      
            
        } 

        public ActionResult Edit(int id) 
        {
            return View(_petsManagementService.GetPet(id));
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

        private ViewResult ValidatePhoto(Pet pet, IFormFile photo)
        {
            if(photo != null)
            {
                if(IsAcceptPhotoType(photo.ContentType, _appSettings.AcceptPhotoTypes))
                {
                    if(photo.Length > 0)
                    {
                        pet.Photo = _petsManagementService.ConvertPhotoToByteArray(photo.OpenReadStream());
                        return null;
                    }
                    else
                    {
                        ViewData["ValidationPhotoAlert"] = "WrongFileLength";
                        return View(pet);
                    }
                }
                else
                {
                    ViewData["ValidationPhotoAlert"] = "WrongFileType";
                    return View(pet); 
                }
            }
            else
            {
                ViewData["ValidationPhotoAlert"] = "NoFileSelected";
                return View(pet);
            }
        }
    }
	
}
