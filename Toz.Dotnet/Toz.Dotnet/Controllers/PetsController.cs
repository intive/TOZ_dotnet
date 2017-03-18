using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Toz.Dotnet.Controllers
{
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
		private readonly IStringLocalizer<PetsController> _localizer;
		
        public PetsController(IPetsManagementService petsManagementService, IStringLocalizer<PetsController> localizer)
        {
            _petsManagementService = petsManagementService;
			_localizer = localizer;
        }

        public IActionResult Index()
        {
            List<Pet> pets = _petsManagementService.GetAllPets().Result; 
            return View(pets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(
            [Bind("Name, Type, Sex, Description, Address")] 
            Pet pet)
        {
            if (pet != null && ModelState.IsValid)
            {
                if (_petsManagementService.CreatePet(pet)) 
                {
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
            Pet pet)
        {
            if (pet != null && ModelState.IsValid)
            {
                if (_petsManagementService.UpdatePet(pet)) 
                {
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
    }
	
}
