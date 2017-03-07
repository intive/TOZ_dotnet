using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Controllers {
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
        
        public PetsController(IPetsManagementService petsManagementService)
        {
            _petsManagementService = petsManagementService;
        }  

        public ActionResult Index() {
            return View(_petsManagementService.GetPetsList());
        }
    }
}