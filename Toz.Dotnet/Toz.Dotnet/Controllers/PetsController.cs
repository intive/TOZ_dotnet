using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;
using System;

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
            return View(_petsManagementService.GetAllPets());
        }

        [HttpPost]
        public IActionResult Add(
            [Bind("Name, Type, Sex, Description, Address")] 
            Pet pet)
        {
            if (pet != null && ModelState.IsValid)
            {
                if (_petsManagementService.AddPet(pet)) 
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                } 
            } 
            else
            {
                return NotFound();
            }      
            
        } 

        public IActionResult Add()
        {
            return View(new Pet());
        }

        public ActionResult Edit(int id) 
        {
            return View(_petsManagementService.GetPet(id));
        }
    }
	
}
