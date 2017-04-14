using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IScheduleManagementService
    {
        Task<Schedule> GetSchedule(DateTime startDate, DateTime endDate, CancellationToken cancelationToken = default(CancellationToken));
        Task<Reservation> GetReservation(string id, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> MakeReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdateReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteReservation(Reservation reservation, CancellationToken cancelationToken = default(CancellationToken));
        DateTime GetFirstDayOfWeek(DateTime day);
    }
}
