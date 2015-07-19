using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CurrentWeekNumber
{
    class Program
    {
        static void Main(string[] args)
        {             
            var dt = new DateTime(2016,1,3);
            var weekOfYear = GetWeekOfYear(dt);
            var year = GetYearkForWeek(dt,weekOfYear);   
            Console.WriteLine("Year: " + year);
            Console.WriteLine("Current DT: {0}", dt);
            Console.WriteLine("Current Week of year: {0}", weekOfYear);               
            var firstDayOfWeek = FirstDateOfWeek(year, weekOfYear, CultureInfo.CreateSpecificCulture("en-US"));
            Console.WriteLine("First day of {0}: {1}", weekOfYear, firstDayOfWeek);
            var lastDayOfWeek = LastDateOfWeek(year, weekOfYear, CultureInfo.CreateSpecificCulture("en-US"));
            Console.WriteLine("Last day of {0}: {1}", weekOfYear, lastDayOfWeek);
            Console.WriteLine("--------********---------------");
            var utcTime = new DateTime(2015,1,1,7,0,0,DateTimeKind.Utc);//DateTime.UtcNow;
            var estTime = ConvertToEstTime(utcTime);
            Console.WriteLine(estTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime dt)
        {
            var ci = CultureInfo.CreateSpecificCulture("en-US");
            var dfi = ci.DateTimeFormat;            
            var cal = dfi.Calendar;
            var weekOfYear = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek,
                dfi.FirstDayOfWeek);
            return weekOfYear;
        }

        public static int GetYearkForWeek(DateTime dt, int week)
        {
            if (week >= 52)
            {
                if (dt.Month == 1)
                {
                    return dt.Year - 1;
                }
            }
            else if (week == 1)
            {
                if (dt.Month == 12)
                    return dt.Year + 1;
            }
            return dt.Year;
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, CultureInfo ci)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            var firstWeekDay = jan1.AddDays(daysOffset);
            var firstWeek = ci.Calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, ci.DateTimeFormat.FirstDayOfWeek);
            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        public static DateTime LastDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            var firstWeekDay = jan1.AddDays(daysOffset);
            var firstWeek = ci.Calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, ci.DateTimeFormat.FirstDayOfWeek);
            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }
            var lastDay = firstWeekDay.AddDays(weekOfYear*7).AddDays(7).AddSeconds(-1);
            return lastDay;
        }

        public static DateTime ConvertToEstTime(DateTime utcDateTime)
        {
            var mountain = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var estDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, mountain);
            return estDateTime;
        }
    }
}
