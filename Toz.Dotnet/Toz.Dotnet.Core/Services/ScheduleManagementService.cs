using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        private IRestService _restService;
        public string RequestUri { get; set; }

        public ScheduleManagementService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;

            RequestUri = appSettings.Value.BackendScheduleUrl;
        }

        public async Task<Schedule> GetSchedule(DateTime startDate, DateTime endDate, CancellationToken cancelationToken = default(CancellationToken))
        {
            const int timeFormatIndex = 5;

            string start = startDate.GetDateTimeFormats()[timeFormatIndex];
            string end = endDate.GetDateTimeFormats()[timeFormatIndex];

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
        
    }
}
