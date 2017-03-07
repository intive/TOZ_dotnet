using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Microsoft.Extensions.Localization;

namespace Toz.Dotnet.Controllers {
    public class AddPetController : Controller
    {
        private IPetsManagementService _petsManagementService;
        private readonly IStringLocalizer<AddPetController> _localizer;

        public AddPetController(IPetsManagementService petsManagementService, IStringLocalizer<AddPetController> localizer)
        {
            _petsManagementService = petsManagementService;
            _localizer = localizer;
        }  

        public ActionResult Index()
        {
            return View();
        }
    }
}