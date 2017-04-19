using System;
using System.Globalization;

namespace Toz.Dotnet.Models.ViewModels
{
    public class Week
    {
        public Slot[] Slots = new Slot[14];
        public DateTime DateFrom;
        public DateTime DateTo 
        { 
            get 
            { 
                if(DateFrom != DateTime.MinValue && DateFrom != null)
                {
                    return DateFrom.AddDays(6);
                }
                else
                {
                    throw new InvalidOperationException("DateFrom field is not set!");
                }
            }
        }
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