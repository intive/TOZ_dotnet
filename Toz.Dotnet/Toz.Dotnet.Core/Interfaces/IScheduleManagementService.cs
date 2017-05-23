using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.ViewModels;
using Period = Toz.Dotnet.Models.EnumTypes.Period;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IScheduleManagementService
    {
        Task<List<Week>> GetSchedule(int weekOffset, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<Reservation> GetReservation(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreateReservation(Slot slot, UserBase userBase, string token, CancellationToken cancelationToken = default(CancellationToken));
        //Task<bool> UpdateReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteReservation(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Slot FindSlot(DateTime date, Period timeOfDay, string token, CancellationToken cancelationToken = default(CancellationToken));
    }
}
