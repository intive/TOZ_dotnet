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

namespace Toz.Dotnet.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleManagementService _scheduleManagementService;
        private readonly IUsersManagementService _usersManagementService;
        private readonly IStringLocalizer<ScheduleController> _localizer;
        private readonly AppSettings _appSettings;

        public ScheduleController(IScheduleManagementService scheduleManagementService, IUsersManagementService usersManagementService,
                                  IStringLocalizer<ScheduleController> localizer, IOptions<AppSettings> appSettings)
        {
            _scheduleManagementService = scheduleManagementService;
            _usersManagementService = usersManagementService;
            _localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> Index(int offset, CancellationToken cancellationToken)
        {     
            List<Week> schedule = await _scheduleManagementService.GetSchedule(offset, cancellationToken);
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
                    ViewData["date"] = date.ToString("yyyy/MM/dd");
                    return View(new ReservationToken());
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation(
            [Bind("Date, TimeOfDay, FirstName, LastName")]
            ReservationToken token, CancellationToken cancellationToken)
        {
            Console.WriteLine("SLOT DATE: " + token.Date + ", TIME: " + token.TimeOfDay);
            Console.WriteLine("USER FIRST: " + token.FirstName + ", LAST: " + token.LastName);

            if (ModelState.IsValid)
            {
                Slot slot = _scheduleManagementService.FindSlot(token.Date, token.TimeOfDay);
                //User user = await _usersManagementService.FindUser(token.FirstName, token.LastName, cancellationToken);
                UserBase user = new User()
                {
                    LastName = token.LastName,
                    FirstName = token.FirstName
                };

                if (await _scheduleManagementService.CreateReservation(slot, user, cancellationToken))
                {
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            return View(token);      
        }
        
        public async Task<ActionResult> DeleteReservation(string id, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _scheduleManagementService.DeleteReservation(id, cancellationToken);
            }

            return RedirectToAction("Index");
        }
    }
}
