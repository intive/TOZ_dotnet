using System;
using System.Collections.Generic;
using System.Globalization;

namespace Toz.Dotnet.Models
{
    public class Week
    {
        public DateTime DateFrom;
        public DateTime DateTo;
        public Slot[] Slots = new Slot[14];

        public int WeekNumber 
        { 
            get 
            {
                return CultureInfo.CurrentCulture.Calendar
                    .GetWeekOfYear(DateFrom, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            }
        }
    }
}