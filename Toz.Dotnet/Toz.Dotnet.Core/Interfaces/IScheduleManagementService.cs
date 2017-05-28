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
        int WeekOffset { get; set; }
        Task<List<Week>> PrepareSchedule(string token, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<Week>> GetSchedule(int offset, string token, CancellationToken cancelationToken = default(CancellationToken));
        Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateReservation(Slot slot, string userId, string token, CancellationToken cancellationToken = default(CancellationToken));
        Task<Reservation> GetReservation(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteReservation(string id, string token, CancellationToken cancellationToken = default(CancellationToken));  
    }
}
