using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class ScheduleManagementService : IScheduleManagementService
    {
        private const int TimeFormatIndex = 0;

        private IRestService _restService;
        public string RequestUri { get; set; }

        public ScheduleManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;

            RequestUri = appSettings.Value.BackendScheduleUrl;
        }

        public async Task<Schedule> GetSchedule(DateTime startDate, DateTime endDate, CancellationToken cancelationToken = default(CancellationToken))
        {
            string start = startDate.GetDateTimeFormats()[TimeFormatIndex];
            string end = endDate.GetDateTimeFormats()[TimeFormatIndex];

            var address = $"{RequestUri}/?from={start}&to={end}";
            return await _restService.ExecuteGetAction<Schedule>(address, cancelationToken);
        }

        public async Task<Reservation> GetReservation(string id, CancellationToken cancelationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Reservation>(address, cancelationToken);
        }

        public async Task<bool> MakeReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = RequestUri;
            return await _restService.ExecutePostAction(address, reservation, cancelationToken);
        }

        public async Task<bool> UpdateReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{reservation.Id}";
            return await _restService.ExecutePutAction(address, reservation, cancelationToken);
        }

        public async Task<bool> DeleteReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{reservation.Id}";
            return await _restService.ExecuteDeleteAction(address, reservation, cancelationToken);
        }

        /// <summary>
        /// Returns the first day of the week.
        /// </summary>
        /// <param name="day">Any day of chosen week.</param>
        /// <returns></returns>   
        public DateTime GetFirstDayOfWeek(DateTime day)
        {
            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int delta = firstDayOfWeek - day.DayOfWeek;
            return day.AddDays(delta);
        }
        
    }
}
