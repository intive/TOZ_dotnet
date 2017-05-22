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
        Task<Week> GetSchedule(int weekOffset, CancellationToken cancelationToken = default(CancellationToken));
        Task<Reservation> GetReservation(string id, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateReservation(Slot slot, UserBase userBase, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteReservation(Reservation r, CancellationToken cancellationToken = default(CancellationToken));
        Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancellationToken = default(CancellationToken));
    }
}
