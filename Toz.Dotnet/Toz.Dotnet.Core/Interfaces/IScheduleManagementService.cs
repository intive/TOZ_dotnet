using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models.Schedule;
using Toz.Dotnet.Models.Schedule.ViewModels;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IScheduleManagementService
    {
        Task<List<Week>> GetInitialSchedule(string token, CancellationToken cancellationToken = default(CancellationToken), int numberOfWeeks = 6);
        Task<List<Week>> GetSchedule(int weekOffset, string token, CancellationToken cancelationToken = default(CancellationToken), int numberOfWeeks = 2);
        Task<List<Week>> GetEarlierSchedule(string token, CancellationToken cancelationToken = default(CancellationToken), int numberOfWeeks = 2);
        Task<List<Week>> GetLaterSchedule(string token, CancellationToken cancelationToken = default(CancellationToken), int numberOfWeeks = 2);
        Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateReservation(Slot slot, string userId, string token, CancellationToken cancellationToken = default(CancellationToken));
        Task<Reservation> GetReservation(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteReservation(string id, string token, CancellationToken cancellationToken = default(CancellationToken));  
    }
}
