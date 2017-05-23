using System;

namespace Toz.Dotnet.Models.Schedule.ViewModels
{
    public class ReservationToken
    {
        public DateTime Date { get; set; }
        public EnumTypes.Period TimeOfDay { get; set; }
        public UserBase Volunteer { get; set; }
    }
}
