using System;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models.ViewModels
{
    public class Slot
    {       
        public DateTime Date;
        public string Day { get { return Date.ToString(); } }
        public Period TimeOfDay;
        public string ReservationId;
        public UserBase Volunteer;
    }
}