using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.Schedule;
using Toz.Dotnet.Models.Schedule.ViewModels;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IScheduleManagementService
    {
        Task<List<Week>> GetInitialSchedule(CancellationToken cancellationToken = default(CancellationToken), int numberOfWeeks = 6);
        Task<List<Week>> GetSchedule(int weekOffset, CancellationToken cancelationToken = default(CancellationToken), int numberOfWeeks = 2);
        Task<List<Week>> GetEarlierSchedule(CancellationToken cancelationToken = default(CancellationToken), int numberOfWeeks = 2);
        Task<List<Week>> GetLaterSchedule(CancellationToken cancelationToken = default(CancellationToken), int numberOfWeeks = 2);
        Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateReservation(Slot slot, string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<Reservation> GetReservation(string id, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteReservation(Reservation r, CancellationToken cancellationToken = default(CancellationToken));  
    }
}
