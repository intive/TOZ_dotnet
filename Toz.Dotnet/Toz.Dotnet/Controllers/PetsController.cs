using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Controllers
{
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
        public PetsController(IPetsManagementService petsManagementService)
        {
            _petsManagementService = petsManagementService;
        }

        public IActionResult Index()
        {
            return View(_petsManagementService.GetSamplePetsList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Pet pet)
        {
            _petsManagementService.AddPet(pet);
            return Index(); 
        } 
    }
}
