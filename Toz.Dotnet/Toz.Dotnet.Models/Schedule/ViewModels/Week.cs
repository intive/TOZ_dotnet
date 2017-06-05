using System;
using System.Globalization;

namespace Toz.Dotnet.Models.Schedule.ViewModels
{
    public class Week
    {
        public Slot[] Slots = new Slot[14];
        public DateTime DateFrom;

        public DateTime DateTo
        {
            get
            {
                if (DateFrom != DateTime.MinValue)
                {
                    return DateFrom.AddDays(6);
                }
                else
                {
                    throw new InvalidOperationException("DateFrom field is not set!");
                }
            }
        }
        public int WeekNumber => GetIso8601WeekOfYear(DateFrom);
        
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}