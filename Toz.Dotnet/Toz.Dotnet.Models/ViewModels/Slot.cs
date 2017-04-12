using System;
using System.Collections.Generic;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models
{
    public struct Slot
    {       
        public DateTime Date;
        public string Day { get { return Date.ToString(); } }
        public Period TimeOfDay;
        public string ReservationId;
        public User Volunteer;
    }
}