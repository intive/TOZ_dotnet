using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Controllers
{
    public class HomeController : Controller
    {
        private IPetsManagementService _petsManagementService;
        public HomeController(IPetsManagementService petsManagementService)
        {
            _petsManagementService = petsManagementService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
