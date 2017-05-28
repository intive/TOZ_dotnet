using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.ViewModels;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.Schedule.ViewModels;
using Toz.Dotnet.Resources.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Toz.Dotnet.Authorization;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Controllers
{
    public class ScheduleController : TozControllerBase<ScheduleController>
    {
        private readonly IScheduleManagementService _scheduleManagementService;
        private readonly IUsersManagementService _usersManagementService;

        public ScheduleController(
            IScheduleManagementService scheduleManagementService, 
            IUsersManagementService usersManagementService,
            IStringLocalizer<ScheduleController> localizer, 
            IOptions<AppSettings> appSettings, 
            IBackendErrorsService backendErrorsService, 
            IAuthService authService) : base(backendErrorsService, localizer, appSettings, authService)
        {
            _scheduleManagementService = scheduleManagementService;
            _usersManagementService = usersManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken, int offset = 0, bool firstLoad = false)
        {
            if (offset == 0 && firstLoad)
            {
                List<Week> schedule = await _scheduleManagementService.PrepareSchedule(AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
                return View(schedule);
            }
            else
            {
                List<Week> schedule = await _scheduleManagementService.GetSchedule(offset, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
                return View(schedule);
            }       
        }

        public IActionResult Earlier(CancellationToken cancellationToken) => RedirectToAction("Index", new { offset = --_scheduleManagementService.WeekOffset } );
        
        public IActionResult Later(CancellationToken cancellationToken) => RedirectToAction("Index", new { offset = ++_scheduleManagementService.WeekOffset } );

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

           List<User> volunteers = (await _usersManagementService.GetAllUsers(AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken))
                .Where(u => u.Roles.Contains(UserType.Volunteer))
                .OrderBy(u => u.LastName)
                .ToList();

            List<UserViewModel> listedUsers = new List<UserViewModel>();
            foreach (var user in volunteers)
            {
                listedUsers.Add(new UserViewModel
                {
                    TheUser = user
                });
            }

            var viewModel = new ReservationViewModel()
            {
                ReservationDate = date.ToString("yyyy-MM-dd"),
                TimeOfDay = timeOfDay,
                Volunteers = new SelectList(listedUsers, "TheUser.Id", "ReadableName")
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReservation(ReservationViewModel viewModel, CancellationToken cancellationToken)
        {
            var isValid = DateTime.TryParse(viewModel.ReservationDate, out DateTime tokenDate);
            if (!isValid)
            {
                ModelState.AddModelError("Date", $"Invalid date ({viewModel.ReservationDate})");
            }
            if (ModelState.IsValid)
            {
                Slot slot = _scheduleManagementService.FindSlot(tokenDate, viewModel.TimeOfDay);

                if (await _scheduleManagementService.CreateReservation(slot, viewModel.VolunteerId, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken))
                {
                    return Json(new {success = true});
                }
                CheckUnexpectedErrors();
            }
            return PartialView(viewModel);
        }

        public async Task<ActionResult> DeleteReservation(string id, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(id) &&
                await _scheduleManagementService.DeleteReservation(id,
                    AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken))
            {
                return RedirectToAction("Index");
            }
            CheckUnexpectedErrors();
            return BadRequest();
        }
    }
}
