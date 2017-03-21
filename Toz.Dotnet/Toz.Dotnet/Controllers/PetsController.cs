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
        private static byte[] lastAcceptPhoto;
		
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
                if (await _petsManagementService.CreatePet(pet)) 
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
        public async Task<IActionResult> Edit(
            [Bind("Id, Name, Type, Sex, Description, Address, AddingTime")] 
            Pet pet, [Bind("Photo")] IFormFile photo, CancellationToken cancellationToken)
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
                if (await _petsManagementService.UpdatePet(pet)) 
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
                        ViewData["ValidationPhotoAlert"] = "EmptyFile";
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
