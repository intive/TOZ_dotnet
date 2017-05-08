using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public class UsersController : Controller
    {
        private IUsersManagementService _usersManagementService;
        private IBackendErrorsService _backendErrorsService;
        private readonly IStringLocalizer<UsersController> _localizer;
        private readonly AppSettings _appSettings;

        public UsersController(IUsersManagementService usersManagementService, IStringLocalizer<UsersController> localizer,
            IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService)
        {
            _usersManagementService = usersManagementService;
            _localizer = localizer;
            _appSettings = appSettings.Value;
            _backendErrorsService = backendErrorsService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<User> users = await _usersManagementService.GetAllUsers();
            return View(users);
        }

        public IActionResult Add()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("FirstName, LastName, PhoneNumber, Email, Purpose")] 
            User user, CancellationToken cancellationToken)
        {
            if (user != null && ModelState.IsValid)
            {   
                if(await _usersManagementService.CreateUser(user, cancellationToken))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _backendErrorsService.UpdateModelState(ModelState);
                }
            }
            return View(user);
        }
    }
}