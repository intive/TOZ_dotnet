using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Models.Schedule;
using Toz.Dotnet.Models.Schedule.ViewModels;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Core.Services
{
    public class ScheduleManagementService : IScheduleManagementService
    {
        public string RequestUri { get; set; }

        private readonly IRestService _restService;
        private readonly IUsersManagementService _usersManagementService;

        private const int DaysInWeek = 7;
        private const int SlotsPerWeek = 14;

        // Monday of the present calendar week       
        private DateTime _datum;
        // Offset (in weeks) from the present calendar week to the week that is currently being viewed
        private int _currentOffset;

        // Stores all downloaded weeks to speed up browsing.
        private List<Week> _cache;
        // Cache index of the present calendar week
        private int _currentWeekIndex;


        public ScheduleManagementService(IRestService restService, IUsersManagementService usersManagementService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            _usersManagementService = usersManagementService;
            RequestUri = appSettings.Value.BackendScheduleUrl;
        }

        /// <summary>
        /// Downloads schedule for the specified number of weeks. 
        /// Starts with the current week and gets maximum 2 weeks from the past. The remaining weeks are always from the future.
        /// </summary>
        /// <param name="numberOfWeeks">Number of weeks to obtain (current week included).</param>
        public async Task<List<Week>> GetInitialSchedule(CancellationToken cancellationToken = default(CancellationToken), int numberOfWeeks = 6)
        {
            if (numberOfWeeks <= 0)
            {
                throw new ArgumentException($"The specified, required number of weeks ({numberOfWeeks}) is not valid!");
            }

            _datum = GetFirstDayOfWeek(DateTime.Today);
            _currentOffset = 0;
            _cache = new List<Week>();

            // Computes the required week counts:
            int pastWeeksCount = (numberOfWeeks < 6) ? numberOfWeeks / 3 : 2;
            int futureWeeksCount = numberOfWeeks - pastWeeksCount - 1;

            // The current week will have the initial index (place in the cache) equal to pastWeeksCount:
            _currentWeekIndex = pastWeeksCount;

            // Creates the ViewModel structure for the obtained data:
            for (int offset = -pastWeeksCount; offset <= futureWeeksCount; offset++)
            {
                int i = offset + pastWeeksCount;
                _cache[i] = new Week { DateFrom = _datum.AddDays(offset * DaysInWeek) };

                for (int j = 0; j < SlotsPerWeek; j++)
                {
                    _cache[i].Slots[j] = new Slot
                    {
                        Date = _cache[i].DateFrom.AddDays(Math.Round(j/2f)),
                        TimeOfDay = (j % 2 == 0) ? Period.Morning : Period.Afternoon
                    };
                }
            }

            // Grabs all entries for specified number of weeks from Backend:
            string dateFrom = _datum.AddDays(-pastWeeksCount * DaysInWeek).ToString("yyyy-MM-dd");
            string dateTo = _datum.AddDays(futureWeeksCount * DaysInWeek - 1).ToString("yyyy-MM-dd");
            var address = $"{RequestUri}/?from={dateFrom}&to={dateTo}";
            Schedule schedule = await _restService.ExecuteGetAction<Schedule>(address, cancellationToken);

            // Fills the structure with the entries:
            foreach (var reservation in schedule.Reservations)
            {
                Slot s = FindSlot(DateTime.Parse(reservation.Date), reservation.StartTime.Equals("08:00") ? Period.Morning : Period.Afternoon, cancellationToken);
                s.ReservationId = reservation.Id;
                s.Volunteer = await _usersManagementService.GetUser(reservation.OwnerId, cancellationToken);
            }

            return _cache;
        }

        /// <summary>
        /// Gets schedule for the selected week.
        /// </summary>
        /// <param name="weekOffset">Offset (in weeks) from the present calendar week to the selected one.</param>
        public async Task<Week> GetSchedule(int weekOffset, CancellationToken cancellationToken = default(CancellationToken))
        {
            // return if Week already in the cache:
            int indexSought = _currentWeekIndex + weekOffset;
            if (indexSought >= 0 && indexSought < _cache.Count)
            {
                return _cache[indexSought];
            }

            _currentOffset += weekOffset;
            _currentWeekIndex += (weekOffset < 0) ? 1 : 0;

            Week newWeek = new Week {DateFrom = _datum.AddDays(weekOffset * DaysInWeek)};

            for (int j = 0; j < SlotsPerWeek; j++)
            {
                newWeek.Slots[j] = new Slot
                {
                    Date = newWeek.DateFrom.AddDays(Math.Round(j / 2f)),
                    TimeOfDay = (j % 2 == 0) ? Period.Morning : Period.Afternoon
                };
            }
                
            _cache.Insert(weekOffset < 0 ? 0 : _cache.Count-1, newWeek);

            string dateFrom = _datum.AddDays(weekOffset * DaysInWeek).ToString("yyyy-MM-dd");
            string dateTo = _datum.AddDays((weekOffset + 1) * DaysInWeek - 1).ToString("yyyy-MM-dd");
            var address = $"{RequestUri}/?from={dateFrom}&to={dateTo}";
            Schedule schedule = await _restService.ExecuteGetAction<Schedule>(address, cancellationToken);

            foreach (var reservation in schedule.Reservations)
            {
                Slot s = FindSlot(DateTime.Parse(reservation.Date), reservation.StartTime.Equals("08:00") ? Period.Morning : Period.Afternoon, cancellationToken);
                s.ReservationId = reservation.Id;
                s.Volunteer = await _usersManagementService.GetUser(reservation.OwnerId, cancellationToken);
            }

            return newWeek;
        }

        public async Task<Reservation> GetReservation(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Reservation>(address, cancellationToken);
        }

        public async Task<bool> CreateReservation(Slot slot, UserBase userData, CancellationToken cancellationToken = default(CancellationToken))
        {
            User user = await _usersManagementService.GetUser(userData.Id, cancellationToken);

            slot.Volunteer = user;

            // Creates a reservation for the user and adds it to reservation pool
            long timeStamp = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            Reservation reservation = new Reservation
            {
                OwnerId = user.Id,
                OwnerName = user.FirstName,
                OwnerSurname = user.LastName,
                Date = slot.Date.ToString("yyyy-MM-dd"),
                StartTime = (slot.TimeOfDay == Period.Morning) ? "08:00" : "12:00",
                EndTime = (slot.TimeOfDay == Period.Morning) ? "12:00" : "16:00",
                Created = timeStamp,
                LastModified = timeStamp
            };

            slot.ReservationId = reservation.Id;

            var address = RequestUri;
            return await _restService.ExecutePostAction(address, reservation, cancellationToken);
        }

        public async Task<bool> DeleteReservation(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{id}";
            //return await _restService.ExecuteDeleteAction<Reservation>(address, cancellationToken);
        }

        public Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancellationToken = default(CancellationToken))
        {
            Week week = _cache.First(w => w.DateFrom == GetFirstDayOfWeek(date));
            return week.Slots.First(s => s.Date == date && s.TimeOfDay == timeOfDay);
        }

        private DateTime GetFirstDayOfWeek(DateTime input)
        {
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays(delta > 0 ? delta - DaysInWeek : delta);
            return monday;
        }
    }
}
