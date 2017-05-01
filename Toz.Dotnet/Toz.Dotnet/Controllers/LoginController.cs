using System;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Toz.Dotnet.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService; // TEMPORARY

        public LoginController(IAuthService authService)
        {
            _authService = authService; // TEMPORARY
        }

        public async Task<IActionResult> Index()
        {
            // --> TEMPORARY
            for(int i = 0; i < 5; i++)
            {
                await _authService.SignIn(new Models.Login() { email = $"TOZ_user{i}.email@gmail.com", password = $"TOZ_name_{i}" });
                if(_authService.IsAuth)
                {
                    ViewData["Auth"] = "Zalogowano!!!";
                    return View();
                }
            }
            ViewData["Auth"] = "Nie zalogowano!!!";
            return View();
            // <--
        }
    }
}
