using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Toz.Dotnet.Resources.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Globalization;

namespace Toz.Dotnet.Controllers
{
    public class ScheduleController : Controller
    {
        private IScheduleManagementService _scheduleManagementService;
        private IUsersManagementService _usersManagementService;
        private readonly IStringLocalizer<ScheduleController> _localizer;
        private readonly AppSettings _appSettings;
        private DateTime _startDate;
                
        public ScheduleController(IScheduleManagementService scheduleManagementService, IUsersManagementService usersManagementService,
                                  IStringLocalizer<ScheduleController> localizer, IOptions<AppSettings> appSettings)
        {
            _scheduleManagementService = scheduleManagementService;
            _usersManagementService = usersManagementService;
            _localizer = localizer;
            _appSettings = appSettings.Value;

            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int delta = firstDayOfWeek - DateTime.Now.DayOfWeek;
            _startDate = DateTime.Now.AddDays(delta);
        }

        public async Task<IActionResult> Index(DateTime startDate, CancellationToken cancellationToken)
        {
            if (startDate != DateTime.MinValue)
                _startDate = startDate;

            const int daysCount = 14;
            Schedule schedule = await _scheduleManagementService.GetSchedule(_startDate, _startDate.AddDays(daysCount - 1));
            return View(schedule);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReservation(
            [Bind("FirstName, LastName")]
            User userData, CancellationToken cancellationToken)
        {
            User user = null;

            if (userData != null && ModelState.IsValid)
            {
                user = await _usersManagementService.FindUser(userData.FirstName, userData.LastName);

                if (user == null)
                {
                    await _usersManagementService.CreateUser(userData);
                    user = await _usersManagementService.FindUser(userData.FirstName, userData.LastName);

                    if (user == null)
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {
                return View(userData);
            }

            Reservation reservation = new Reservation();
            reservation.OwnerId = user.Id;
            reservation.ModificationMessage = String.Format("Assigned to {0} {1}.", user.FirstName, user.LastName);
            //Get number of slot
            //Based on a chosen Period assign Date, EndTime, StartTime to reservation object
            //Assign ModificationAuthorId (Currenlty logged in user) to reservation object.

            if (await _scheduleManagementService.MakeReservation(reservation))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest();
            }
            
        }

        public IActionResult AddReservation()
        {
            //Store information about slot number  
            return View(new User());
        }
        
        public async Task<ActionResult> DeleteReservation(string id, CancellationToken cancellationToken)
        {
            var reservation = await _scheduleManagementService.GetReservation(id);
            var user = await _usersManagementService.GetUser(reservation.OwnerId);

            if (reservation != null)
            {
                reservation.ModificationMessage = String.Format("Cancelled reservation for {0} {1}.", user.FirstName, user.LastName);
                //Assign ModificationAuthorId (Currenlty logged in user) to reservation object
                await _scheduleManagementService.DeleteReservation(reservation);
            }

            return RedirectToAction("Index");
        }
    }
}
