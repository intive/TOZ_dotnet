using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;


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
            return View(_petsManagementService.GetSamplePets());
        }

        [HttpPost]
        public IActionResult Add([FromForm] Pet pet)
        {
            _petsManagementService.AddPet(pet);
            return Index(); 
        } 

        public ActionResult Edit(int id) 
        {
            return View(_petsManagementService.GetPet(id));
        }
    }
	
}
