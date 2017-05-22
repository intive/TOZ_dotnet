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
using Toz.Dotnet.Models.Schedule.ViewModels;
using Toz.Dotnet.Resources.Configuration;
using System.Globalization;
using System.Linq;
using Toz.Dotnet.Models.Schedule;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleManagementService _scheduleManagementService;
        private readonly IUsersManagementService _usersManagementService;
        private readonly IBackendErrorsService _backendErrorsService;
        private readonly IStringLocalizer<ScheduleController> _localizer;
        private readonly AppSettings _appSettings;

        public ScheduleController(IScheduleManagementService scheduleManagementService, IUsersManagementService usersManagementService,
                                  IStringLocalizer<ScheduleController> localizer, IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService)
        {
            _scheduleManagementService = scheduleManagementService;
            _usersManagementService = usersManagementService;
            _localizer = localizer;
            _appSettings = appSettings.Value;
            _backendErrorsService = backendErrorsService;
        }

        public async Task<IActionResult> Index(int offset, CancellationToken cancellationToken)
        {     
            List<Week> schedule = await _scheduleManagementService.GetInitialSchedule(cancellationToken);
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

        public async Task<IActionResult> AddReservation(DateTime date, Period timeOfDay, CancellationToken cancellationToken)
        {
            if (date <= DateTime.MinValue || date >= DateTime.MaxValue)
            {
                return BadRequest();
            }

            if (timeOfDay != Period.Afternoon && timeOfDay != Period.Morning)
            {
                return BadRequest();
            }

           List<User> volunteers = (await _usersManagementService.GetAllUsers(cancellationToken))
                .Where(u => u.Roles.Contains(UserType.Volunteer))
                .OrderBy(u => u.LastName)
                .ToList();

            ViewBag.Volunteers = volunteers;

            var token = new ReservationToken()
            {
                Date = date
            };

            return PartialView(token);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReservation(ReservationToken token, CancellationToken cancellationToken)
        {
            token.Date = Convert.ToDateTime(token.Date.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            if (ModelState.IsValid)
            {
                Slot slot = _scheduleManagementService.FindSlot(token.Date, token.TimeOfDay);
                
                if (await _scheduleManagementService.CreateReservation(slot, token.Volunteer, cancellationToken))
                {
                    return Json(new { success = true });
                }

                var overallError = _backendErrorsService.UpdateModelState(ModelState);
                if (!string.IsNullOrEmpty(overallError))
                {
                    ViewData["UnhandledError"] = overallError;
                }
            }

            return PartialView(token);      
        }
        
        public async Task<ActionResult> DeleteReservation(string id, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Reservation r = await _scheduleManagementService.GetReservation(id, cancellationToken);
                await _scheduleManagementService.DeleteReservation(r, cancellationToken);
            }

            return RedirectToAction("Index");
        }
    }
}
