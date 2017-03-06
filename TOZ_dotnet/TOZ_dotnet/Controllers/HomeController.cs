using Microsoft.AspNetCore.Mvc;
using TOZ_dotnet.Core.Interfaces;

namespace MvcApp.Controllers
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

        public IActionResult About()
        {
            ViewData["Message"] = "Hello Patronage 2017 .NET Team! " + _petsManagementService.GetTestString();

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
