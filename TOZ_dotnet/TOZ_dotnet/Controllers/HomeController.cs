using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TOZ_dotnet.Core;
using TOZ_dotnet.Core.Interfaces;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private IAnimalsManagementService _animalsManagementService;
        public HomeController(IAnimalsManagementService animalsManagementService)
        {
            _animalsManagementService = animalsManagementService;
        }

        


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Hello Patronage 2017 .NET Team! " + _animalsManagementService.GetTestString();

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
