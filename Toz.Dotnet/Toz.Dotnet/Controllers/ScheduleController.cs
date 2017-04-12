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
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public class ScheduleController : Controller
    {
        private IUsersManagementService _usersManagementService;
        private readonly IStringLocalizer<UsersController> _localizer;
        private readonly AppSettings _appSettings;

        public ScheduleController(IUsersManagementService usersManagementService, IStringLocalizer<UsersController> localizer, IOptions<AppSettings> appSettings)
        {
            _usersManagementService = usersManagementService;
            _localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> AddReservation()
        {
            var u = new User();
            return View(u);
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        { 
            // Fake setup for a 2-weeks schedule with no reservations
            var Week1 = new Week();
            var Week2 =  new Week();
            Week1.DateFrom = new DateTime(2017, 4, 3);
            Week1.DateTo = new DateTime(2017, 4, 9);
            Week2.DateFrom = new DateTime(2017, 4, 10);
            Week2.DateTo = new DateTime(2017, 4, 16);
            for(int i=0; i<14; i++)
            {
                var slot1 = Week1.Slots[i] = new Slot();
                slot1.Date = new DateTime(2017, 4, Week1.DateFrom.Day + i);
                var slot2 = Week2.Slots[i] = new Slot();
                slot2.Date = new DateTime(2017, 4, Week2.DateFrom.Day + i);

                slot1.TimeOfDay = slot1.TimeOfDay = (i%2 == 1) ? Period.Morning : Period.Afternoon;
            }
            var WeekList = new List<Week>();
            WeekList.Add(Week1);
            WeekList.Add(Week2);
            // example reservation
            Week1.Slots[2].Volunteer = await _usersManagementService.GetUser("0");
            // end of fake setup

            return View(WeekList);
        }
    }
}