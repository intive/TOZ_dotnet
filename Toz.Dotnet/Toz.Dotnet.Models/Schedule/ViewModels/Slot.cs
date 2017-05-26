using System;

namespace Toz.Dotnet.Models.Schedule.ViewModels
{
    public class Slot
    {       
        public DateTime Date;
        public string Day => Date.ToString("yyyy-MM-dd");
        public EnumTypes.Period TimeOfDay;
        public string ReservationId;
        public UserBase Volunteer;
    }
}