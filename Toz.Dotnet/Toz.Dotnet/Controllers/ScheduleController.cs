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
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {     
            List<Week> schedule = await _scheduleManagementService.GetInitialSchedule(cancellationToken);
            return View(schedule);
        }

        public async Task<IActionResult> Earlier(CancellationToken cancellationToken)
        {
            List<Week> earlierSchedule = await _scheduleManagementService.GetEarlierSchedule(cancellationToken);
            return View("Index", earlierSchedule);
        }

        public async Task<IActionResult> Later(CancellationToken cancellationToken)
        {
            List<Week> laterSchedule = await _scheduleManagementService.GetLaterSchedule(cancellationToken);
            return View("Index", laterSchedule);
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

            var token = new ReservationToken()
            {
                Date = date,
                TimeOfDay = timeOfDay,
                Volunteers = new SelectList(volunteers, "Id", "CombinedName")
            };

            return PartialView(token);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReservation(ReservationToken token, CancellationToken cancellationToken)
        {
            if (token != null && ModelState.IsValid)
            {
                Slot slot = _scheduleManagementService.FindSlot(token.Date, token.TimeOfDay);

                if (await _scheduleManagementService.CreateReservation(slot, token.VolunteerId, cancellationToken))
                {
                    return Json(new {success = true});
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
