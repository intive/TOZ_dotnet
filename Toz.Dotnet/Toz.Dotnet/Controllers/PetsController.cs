using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Microsoft.Extensions.Localization;

namespace Toz.Dotnet.Controllers {
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
        private readonly IStringLocalizer<PetsController> _localizer;

        public PetsController(IPetsManagementService petsManagementService, IStringLocalizer<PetsController> localizer)
        {
            _petsManagementService = petsManagementService;
            _localizer = localizer;
        }  

        public ActionResult Index() 
        {
            return View(_petsManagementService.GetPetsList());
        }
    }
}