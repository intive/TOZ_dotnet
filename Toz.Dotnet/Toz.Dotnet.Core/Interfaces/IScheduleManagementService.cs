using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.ViewModels;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IScheduleManagementService
    {
        Task<List<Week>> GetSchedule(int weekOffset, CancellationToken cancelationToken = default(CancellationToken));
        Task<Reservation> GetReservation(string id, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateReservation(Slot slot, UserBase userBase, CancellationToken cancelationToken = default(CancellationToken));
        //Task<bool> UpdateReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteReservation(string id, CancellationToken cancelationToken = default(CancellationToken));
        Slot FindSlot(DateTime date, Period timeOfDay, CancellationToken cancelationToken = default(CancellationToken));
    }
}
