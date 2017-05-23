using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;
using System.Linq;
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
                var newWeek = new Week { DateFrom = _datum.AddDays(offset * DaysInWeek) };
                for (int j = 0; j < SlotsPerWeek; j++)
                {
                    newWeek.Slots[j] = new Slot
                    {
                        Date = newWeek.DateFrom.AddDays(Math.Floor(j / 2f)),
                        TimeOfDay = (j % 2 == 0) ? Period.Morning : Period.Afternoon
                    };
                }
                _cache.Add(newWeek);
            }

            // Grabs all entries for specified number of weeks from Backend:
            string dateFrom = _datum.AddDays(-pastWeeksCount * DaysInWeek).ToString("yyyy-MM-dd");
            string dateTo = _datum.AddDays(futureWeeksCount * DaysInWeek - 1).ToString("yyyy-MM-dd");
            var address = $"{RequestUri}/?from={dateFrom}&to={dateTo}";
            Schedule schedule = await _restService.ExecuteGetAction<Schedule>(address, cancellationToken);

            // Fills the structure with the entries:
            foreach (var reservation in schedule.Reservations)
            {
                Slot s = FindSlot(DateTime.ParseExact(reservation.Date, "yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo), reservation.StartTime.Equals("08:00") ? Period.Morning : Period.Afternoon, cancellationToken);
                s.ReservationId = reservation.Id;
                s.Volunteer = await _usersManagementService.GetUser(reservation.OwnerId, cancellationToken);
            }

            return new List<Week>
            {
                _cache[_currentWeekIndex],
                _cache[_currentWeekIndex + 1]
            };
        }

        /// <summary>
        /// Gets schedule for the selected week and the given number of following weeks (default: 2).
        /// </summary>
        /// <param name="firstWeekOffset">Offset (in weeks) from the present calendar week to the selected one.</param>
        /// <param name="numberOfWeeks">Number of consecutive weeks to return (starting from the selected week).</param>
        public async Task<List<Week>> GetSchedule(int firstWeekOffset, CancellationToken cancellationToken = default(CancellationToken), int numberOfWeeks = 2)
        {
            List<Week> output = new List<Week>();
            for (int i = 0; i < numberOfWeeks; i++)
            {
                output.Add(await GetWeek(firstWeekOffset + i, cancellationToken));
            }
            return output;
        }

        /// <summary>
        /// Shifts the schedule one week forward and returns selected number of weeks (default: 2)
        /// </summary>
        public async Task<List<Week>> GetLaterSchedule(CancellationToken cancellationToken = default(CancellationToken), int numberOfWeeks = 2)
        {
            return await GetSchedule(++_currentOffset, cancellationToken, numberOfWeeks);
        }

        /// <summary>
        /// Shifts the schedule one week backward and returns selected number of weeks (default: 2)
        /// </summary>
        public async Task<List<Week>> GetEarlierSchedule(CancellationToken cancellationToken = default(CancellationToken), int numberOfWeeks = 2)
        {
            return await GetSchedule(--_currentOffset, cancellationToken, numberOfWeeks);
        }

        public async Task<Reservation> GetReservation(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Reservation>(address, cancellationToken);
        }

        public async Task<bool> CreateReservation(Slot slot, UserBase userData, CancellationToken cancellationToken = default(CancellationToken))
        {
            User user = await _usersManagementService.GetUser(userData.Id, cancellationToken);

            var timeStamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
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

            var address = RequestUri;
            string id = await _restService.ExecutePostActionAndReturnId(address, reservation, cancellationToken);

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            slot.ReservationId = id;
            slot.Volunteer = user;
            return true;
        }

        public async Task<bool> DeleteReservation(Reservation r, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{r.Id}";
            return await _restService.ExecuteDeleteAction(address, r, cancellationToken);
        }

        public Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancellationToken = default(CancellationToken))
        {
            Week week = _cache.First(w => w.DateFrom == GetFirstDayOfWeek(date));
            return week.Slots.First(s => s.Date == date && s.TimeOfDay == timeOfDay);
        }

        private async Task<Week> GetWeek(int weekOffset, CancellationToken cancellationToken = default(CancellationToken))
        {
            // return if Week already in the cache:
            int indexSought = _currentWeekIndex + weekOffset;
            if (indexSought >= 0 && indexSought < _cache.Count)
            {
                return _cache[indexSought];
            }

            _currentOffset = weekOffset;
            _currentWeekIndex += (weekOffset < 0) ? 1 : 0;

            Week newWeek = new Week
            {
                DateFrom = _datum.AddDays(weekOffset * DaysInWeek)
            };

            for (int j = 0; j < SlotsPerWeek; j++)
            {
                newWeek.Slots[j] = new Slot
                {
                    Date = newWeek.DateFrom.AddDays(Math.Floor(j / 2f)),
                    TimeOfDay = (j % 2 == 0) ? Period.Morning : Period.Afternoon
                };
            }

            if (weekOffset < 0)
            {
                _cache.Insert(0, newWeek);
            }
            else
            {
                _cache.Add(newWeek);
            }

            int daysFromDatum = weekOffset * DaysInWeek;
            string dateFrom = _datum.AddDays(daysFromDatum).ToString("yyyy-MM-dd");
            string dateTo = _datum.AddDays(daysFromDatum + DaysInWeek - 1).ToString("yyyy-MM-dd");
            var address = $"{RequestUri}/?from={dateFrom}&to={dateTo}";
            Schedule schedule = await _restService.ExecuteGetAction<Schedule>(address, cancellationToken);

            if (schedule?.Reservations != null)
            {
                foreach (var reservation in schedule.Reservations)
                {
                    Slot s = FindSlot(DateTime.ParseExact(reservation.Date, "yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo), reservation.StartTime.Equals("08:00") ? Period.Morning : Period.Afternoon, cancellationToken);
                    s.ReservationId = reservation.Id;
                    s.Volunteer = await _usersManagementService.GetUser(reservation.OwnerId, cancellationToken);
                }
            }
            return newWeek;
        }

        private DateTime GetFirstDayOfWeek(DateTime input)
        {
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays(delta > 0 ? delta - DaysInWeek : delta);
            return monday;
        }
    }
}
