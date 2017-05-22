using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.ViewModels;
using Toz.Dotnet.Resources.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Toz.Dotnet.Authorization;

namespace Toz.Dotnet.Controllers
{
    public class ScheduleController : TozControllerBase<ScheduleController>
    {
        private readonly IScheduleManagementService _scheduleManagementService;
        private readonly IUsersManagementService _usersManagementService;

        public ScheduleController(IScheduleManagementService scheduleManagementService, IUsersManagementService usersManagementService,
                                  IStringLocalizer<ScheduleController> localizer, IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService, IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _scheduleManagementService = scheduleManagementService;
            _usersManagementService = usersManagementService;
        }

        public async Task<IActionResult> Index(int offset, CancellationToken cancellationToken)
        {     
            List<Week> schedule = await _scheduleManagementService.GetSchedule(offset, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName), cancellationToken);
            return View(schedule);
        }

        public IActionResult Earlier()
        {
            return RedirectToAction("Index", new {offset = -1});
        }

        public IActionResult Later()
        {
            return RedirectToAction("Index", new {offset = +1});
        }

        public IActionResult AddReservation(DateTime date, Period timeOfDay)
        { 
            if (date > DateTime.MinValue && date < DateTime.MaxValue)
            {
                if (timeOfDay == Period.Afternoon || timeOfDay == Period.Morning)
                {
                    ReservationToken token = new ReservationToken()
                    {
                        Date = date
                    };
                    
                    return PartialView(token);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReservation(
            [Bind("Date, TimeOfDay, FirstName, LastName")]
            ReservationToken token, CancellationToken cancellationToken)
        {
            Console.WriteLine("SLOT DATE: " + token.Date + ", TIME: " + token.TimeOfDay);
            Console.WriteLine("USER FIRST: " + token.FirstName + ", LAST: " + token.LastName);

            token.Date = Convert.ToDateTime(token.Date.ToString(), CultureInfo.InvariantCulture);

            if (ModelState.IsValid)
            {
                Slot slot = _scheduleManagementService.FindSlot(token.Date, token.TimeOfDay, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName));
                //User user = await _usersManagementService.FindUser(token.FirstName, token.LastName, cancellationToken);
                UserBase user = new User()
                {
                    LastName = token.LastName,
                    FirstName = token.FirstName
                };

                if (await _scheduleManagementService.CreateReservation(slot, user, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName), cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
            }

            return PartialView(token);      
        }
        
        public async Task<ActionResult> DeleteReservation(string id, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _scheduleManagementService.DeleteReservation(id, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName), cancellationToken);
            }

            return RedirectToAction("Index");
        }
    }
}
