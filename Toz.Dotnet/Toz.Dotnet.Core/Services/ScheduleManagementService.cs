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
using System.Text.RegularExpressions;
using Toz.Dotnet.Models.Schedule;
using Toz.Dotnet.Models.Schedule.ViewModels;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Core.Services
{
    public class ScheduleManagementService : IScheduleManagementService
    {
        public string RequestUri { get; set; }

        // Offset (in weeks) from the present calendar week to the week that is currently being viewed
        public int WeekOffset
        {
            get => _weekOffset;
            set
            {
                // Prevent the user from manually setting large offsets
                if (Math.Abs(_weekOffset - value) != 1) return;
                _weekOffset = value;
            }
        }
        
        private readonly IRestService _restService;
        private readonly IUsersManagementService _usersManagementService;

        private const int DaysInWeek = 7;
        private const int SlotsPerWeek = 14;
        private const int NumberOfInitiallyCachedWeeks = 6;
        private const int NumberOfWeeksToDisplay = 2;

        // Monday of the present calendar week       
        private DateTime _datum;
        
        // Stores all downloaded weeks to speed up browsing.
        private List<Week> _cache;
        // Cache index of the present calendar week
        private int _currentWeekIndex;
        // Offset of the currently displayed week
        private int _weekOffset;


        public ScheduleManagementService(IRestService restService, IUsersManagementService usersManagementService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            _usersManagementService = usersManagementService;
            RequestUri = appSettings.Value.BackendBaseUrl + appSettings.Value.BackendScheduleUrl;
        }
        
        /// <summary>
        /// Downloads schedule for the specified number of weeks. 
        /// Starts with the current week and gets maximum 2 weeks from the past. The remaining weeks are always from the future.
        /// </summary>
        /// <param name="NumberOfInitiallyCachedWeeks">Number of weeks to obtain (current week included).</param>
        public async Task<List<Week>> PrepareSchedule(string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (NumberOfInitiallyCachedWeeks <= 0)
            {
                throw new ArgumentException($"The specified, required number of weeks ({NumberOfInitiallyCachedWeeks}) is not valid!");
            }

            _datum = GetMonday(DateTime.Today);
            _weekOffset = 0;

            if (_cache?.Count > 0)
            {
                _cache.Clear();
            }
            else
            {
                _cache = new List<Week>();
            }
            
            // Computes the required week counts:
            int pastWeeksCount = (NumberOfInitiallyCachedWeeks < 6) ? NumberOfInitiallyCachedWeeks / 3 : 2;
            int futureWeeksCount = NumberOfInitiallyCachedWeeks - pastWeeksCount - 1;

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
            Schedule schedule = await _restService.ExecuteGetAction<Schedule>(address, token, cancellationToken);

            // Fills the structure with the entries:
            foreach (var reservation in schedule.Reservations)
            {
                Slot s = FindSlot(DateTime.ParseExact(reservation.Date, "yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo), reservation.StartTime.Equals("08:00") ? Period.Morning : Period.Afternoon, cancellationToken);
                s.ReservationId = reservation.Id;
                s.Volunteer = await _usersManagementService.GetUser(reservation.OwnerId, token, cancellationToken);
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
        /// <param name="offset">Offset (in weeks) from the present calendar week to the selected one.</param>
        /// <param name="numberOfWeeks">Number of consecutive weeks to return (starting from the selected week).</param>
        public async Task<List<Week>> GetSchedule(int offset, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            WeekOffset = offset;

            List<Week> output = new List<Week>();
            for (int i = 0; i < NumberOfWeeksToDisplay; i++)
            {
                output.Add(await GetWeek(WeekOffset + i, token, cancellationToken));
            }
            return output;
        }
        
        public async Task<Reservation> GetReservation(string id, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            string address = $"{RequestUri}/{id}";
            return await _restService.ExecuteGetAction<Reservation>(address, token, cancellationToken);
        }

        public async Task<bool> CreateReservation(Slot slot, string userId, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Verify correct userId:
            Regex regex = new Regex("([a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12})");
            Match match = regex.Match(userId);
            if (!match.Success)
            {
                return false;
            }
            
            User user = await _usersManagementService.GetUser(userId, token, cancellationToken);

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
            string id = await _restService.ExecutePostActionAndReturnId(address, reservation, token, cancellationToken);

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            slot.ReservationId = id;
            slot.Volunteer = user;
            return true;
        }

        public async Task<bool> DeleteReservation(string id, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            var address = $"{RequestUri}/{id}";

            // Try to find the sought schedule id within visible slots:
            int weekIndex = _currentWeekIndex + WeekOffset;
            List<Slot> displayedSlots = new List<Slot>();
            for (int i = weekIndex; i < weekIndex + NumberOfWeeksToDisplay; i++)
            {
                displayedSlots.AddRange(_cache[i].Slots);
            }
            Slot affectedSlot = displayedSlots.FirstOrDefault(s => string.Equals(s.ReservationId, id));

            if (affectedSlot == null)
            {
                return false;
            }

            bool success = await _restService.ExecuteDeleteAction(address, token, cancellationToken);
            if (!success)
            {
                return false;
            }
            affectedSlot.ReservationId = string.Empty;
            affectedSlot.Volunteer = null;
            return true;
        }

        public Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancelationToken = default(CancellationToken))
        {
            Week week = _cache.First(w => w.DateFrom == GetMonday(date));
            return week.Slots.First(s => s.Date == date && s.TimeOfDay == timeOfDay);
        }

        private async Task<Week> GetWeek(int weekOffset, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            // return Week if already in the cache:
            int indexSought = _currentWeekIndex + weekOffset;
            if (indexSought >= 0 && indexSought < _cache?.Count)
            {
                return _cache[indexSought];
            }

            // Otherwise create and configure a new Week
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
            Schedule schedule = await _restService.ExecuteGetAction<Schedule>(address, token, cancellationToken);

            if (schedule?.Reservations != null)
            {
                foreach (var reservation in schedule.Reservations)
                {
                    Slot s = FindSlot(DateTime.ParseExact(reservation.Date, "yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo), reservation.StartTime.Equals("08:00") ? Period.Morning : Period.Afternoon, cancellationToken);
                    s.ReservationId = reservation.Id;
                    s.Volunteer = await _usersManagementService.GetUser(reservation.OwnerId, token, cancellationToken);
                }
            }
            return newWeek;
        }
        
        private DateTime GetMonday(DateTime input)
        {
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays(delta > 0 ? delta - DaysInWeek : delta);
            return monday;
        }
    }
}

