using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;
using Microsoft.AspNetCore.Authorization;
using Toz.Dotnet.Authorization;

namespace Toz.Dotnet.Controllers
{
    public class UsersController : TozControllerBase<UsersController>
    {
        private readonly IUsersManagementService _usersManagementService;

        public UsersController(IUsersManagementService usersManagementService, IStringLocalizer<UsersController> localizer,
            IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService, IAuthService authService) : base(backendErrorsService,localizer,appSettings, authService)
        {
            _usersManagementService = usersManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<User> users = await _usersManagementService.GetAllUsers(AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName), cancellationToken);
            return View(users);
        }

        public IActionResult Add()
        {
            return PartialView(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("FirstName, LastName, PhoneNumber, Email, Password, Roles")]
            User user, CancellationToken cancellationToken)
        {
            if (user != null && ModelState.IsValid)
            {   
                if(await _usersManagementService.CreateUser(user, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName), cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
                return PartialView(user);
            }
            return PartialView(user);
        }

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken)
        {
            return PartialView("Edit", await _usersManagementService.GetUser(id, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName), cancellationToken));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id, FirstName, LastName, PhoneNumber, Email, Password, Roles")]
            User user, CancellationToken cancellationToken)
        {
            if (user != null && ModelState.IsValid)
            {
                if (await _usersManagementService.UpdateUser(user, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName), cancellationToken))
                {
                    return Json(new { success = true });
                }
                var overallError = BackendErrorsService.UpdateModelState(ModelState);
                if (!string.IsNullOrEmpty(overallError))
                {
                    this.ViewData["UnhandledError"] = overallError;
                }
                return PartialView(user);
            }
            return PartialView(user);
        }
    }
}