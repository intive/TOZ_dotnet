using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public class UsersController : Controller
    {
        private readonly IStringLocalizer<UsersController> _localizer;
        private readonly AppSettings _appSettings;

        public UsersController(IStringLocalizer<UsersController> localizer, IOptions<AppSettings> appSettings)
        {
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        public ActionResult Add(CancellationToken cacellationToken)
        {
            return View();
        }
    }
}