using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Resources.Configuration;
using System.Linq;
using Toz.Dotnet.Models.ViewModels;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Core.Services
{
    public class ScheduleManagementService : IScheduleManagementService
    {
        public string RequestUri { get; set; }

        private readonly IRestService _restService;
        private readonly IUsersManagementService _usersManagementService;

        // Monday of the current week       
        private DateTime _datum;
        // Offset from the current week
        private int _offset;        
        // As per specification, Backend will return entries from 6 weeks based on the current date.
        private Week[] _cache;    
        //TODO: Delete after connecting with Backend API
        private List<Reservation> _reservationsMockupDb;
        

        public ScheduleManagementService(IRestService restService, IUsersManagementService usersManagementService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            _usersManagementService = usersManagementService;
            _datum = GetFirstDayOfWeek(DateTime.Today);
            RequestUri = appSettings.Value.BackendScheduleUrl;
            //TODO: Delete after connecting with Backend API
            _reservationsMockupDb = new List<Reservation>();
        }

        public async Task<List<Week>> GetSchedule(int weekOffset, CancellationToken cancelationToken = default(CancellationToken))
        {
            _offset += weekOffset;

            if(_cache == null)
            {
                CreateScheduleMockup();
            }

            List<Week> schedule = new List<Week>();

            if (_cache.Length > _offset + 3 && _offset + 2 >= 0)
            {
                schedule.Add(_cache[2 + _offset]);
                schedule.Add(_cache[3 + _offset]);
            }

            return schedule;

            //var address = $"{RequestUri}/?from={_datum.AddDays(7 * weekOffset)}&to={_datum.AddDays(8*weekOffset - 1)}";
            //return await _restService.ExecuteGetAction<Schedule>(address, cancelationToken);
        }

        public async Task<Reservation> GetReservation(string id, CancellationToken cancelationToken = default(CancellationToken))
        {
            return _reservationsMockupDb.Find(p => p.Id == id);
            //string address = $"{RequestUri}/{id}";
            //return await _restService.ExecuteGetAction<Reservation>(address, cancelationToken);
        }

        public async Task<bool> CreateReservation(Slot slot, UserBase userData, CancellationToken cancelationToken = default(CancellationToken))
        {
            User user = await _usersManagementService.FindUser(userData.FirstName, userData.LastName, cancelationToken);

            // Registers the user if not already registered:
            if (user == null)
            {
                user = new User
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName
                };
                if(await _usersManagementService.CreateUser(user, cancelationToken) == false)
                {
                    return false;
                }

                user = await _usersManagementService.FindUser(userData.FirstName, userData.LastName, cancelationToken);
                if (user == null)
                {
                    return false;
                }
            }

            slot.Volunteer = user;

            // Creates a reservation for the user and adds it to reservation pool
            Reservation r = new Reservation
            {
                OwnerId = user.Id,
                Date = slot.Date,
                StartTime = (slot.TimeOfDay == Period.Morning) ? "08:00" : "12:00",
                EndTime = (slot.TimeOfDay == Period.Morning) ? "12:00" : "16:00",
                Created = DateTime.Today
                //TODO: ModificationAuthorId: set to Id of the currently logged in user
            };

            //TODO: remove once BE is ready
            _reservationsMockupDb.Add(r);
            r.Id = _reservationsMockupDb.IndexOf(r).ToString();
            slot.ReservationId = r.Id;
            return true;

            //var address = RequestUri;
            //return await _restService.ExecutePostAction(address, r, cancelationToken);
        }

        public async Task<bool> DeleteReservation(string id, CancellationToken cancelationToken = default(CancellationToken))
        {
            var r = await GetReservation(id, cancelationToken);
            if(r != null)
            {
                foreach(Week w in _cache)
                {
                    Slot slot = w.Slots.FirstOrDefault(s => s.ReservationId == r.Id);
                    if(slot != null)
                    {
                        slot.ReservationId = string.Empty;
                        slot.Volunteer = null;
                        break;
                    }
                }
                _reservationsMockupDb.Remove(r);
                return true;
            }
            return false;
            //var address = $"{RequestUri}/{reservation.Id}";
            //return await _restService.ExecuteDeleteAction(address, reservation, cancelationToken);
        }

        public Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancelationToken = default(CancellationToken))
        {
            Week week = _cache.First(w => w.DateFrom == GetFirstDayOfWeek(date));
            return week.Slots.First(s => s.Date == date && s.TimeOfDay == timeOfDay);
        }

        private DateTime GetFirstDayOfWeek(DateTime input)
        {
            const int countOfDaysInWeek = 7;
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays((delta > 0) ? delta - countOfDaysInWeek : delta);
            return monday;
        }

        // Creates a fake 6-weeks schedule with sample reservations
        private async void CreateScheduleMockup()
        {
            const int countOfWeeks = 6;
            const int countOfSlotsInOneWeek = 14;
            // Set-up weeks
            _cache = new Week[countOfWeeks];  
            for (int i=0; i< countOfWeeks; i++)
            {
                _cache[i] = new Week();
            }

            _cache[0].DateFrom = _datum.AddDays(-14);
            _cache[1].DateFrom = _datum.AddDays(-7);
            _cache[2].DateFrom = _datum;
            _cache[3].DateFrom = _datum.AddDays(7);
            _cache[4].DateFrom = _datum.AddDays(14);
            _cache[5].DateFrom = _datum.AddDays(21);

            // Set-up slots for each week
            foreach(var week in _cache)
            {
                for(int i=0; i< countOfSlotsInOneWeek; i++)
                {
                    week.Slots[i] = new Slot
                    {
                        Date = week.DateFrom.AddDays((int)(i/2)),
                        TimeOfDay = (i%2 == 0) ? Period.Morning : Period.Afternoon             
                    };
                }
            }
            
            /*// Fill the calendar with sample users
            _usersManagementService.SetupSampleUsers();
            Random r = new Random();
            for(int i=0; i<r.Next(5,30); i++)
            {
                Slot randomSlot = _cache[r.Next(0,5)].Slots[r.Next(0,13)];
                randomSlot.Volunteer = _usersManagementService.GetRandomVolunteer();
                await CreateReservation(randomSlot, randomSlot.Volunteer);
            }*/
        }
    }
}
