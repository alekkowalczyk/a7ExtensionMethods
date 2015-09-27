using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7ExtensionMethods
{
    public static class a7DateTimeExtensions
    {
        public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static DateTime GetNearestDayOfWeekBefore(this DateTime date, DayOfWeek dayOfWeek)
        {
            while (date.DayOfWeek != dayOfWeek)
                date = date.AddDays(-1);
            return date;
        }

        public static DateTime GetNearestDayOfWeekAfter(this DateTime date, DayOfWeek dayOfWeek)
        {
            while (date.DayOfWeek != dayOfWeek)
                date = date.AddDays(1);
            return date;
        }

        public static DateTime GetEndOfMonth(this DateTime date)
        {
            DateTime endOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month),23, 59, 59);
            return endOfMonth;
        }

        public static DateTime GetStartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 23, 59, 59);
        }

        /// <summary>
        /// Determines whether a subject date is the same as a date passed in.
        /// </summary>
        /// <param name="subjectDate"> The subject date.</param>
        /// <param name="dateToCompare">The date passed in.</param>
        /// <returns>True if the two DateTime objects represent the same date, even if their time components differ; false otherwise.</returns>
        public static bool IsSameDateAs(this DateTime subjectDate, DateTime dateToCompare)
        {
            var dayIsSame = subjectDate.Day == dateToCompare.Day;
            var monthIsSame = subjectDate.Month == dateToCompare.Month;
            var yearIsSame = subjectDate.Year == dateToCompare.Year;
            return dayIsSame && monthIsSame && yearIsSame;
        }

        /// <summary>
        /// Determines whether a subject date is in the same month as a date passed in.
        /// </summary>
        /// <param name="subjectDate"> The subject date.</param>
        /// <param name="dateToCompare">The date passed in.</param>
        /// <returns>True if the two DateTime objects are in the same month; false otherwise.</returns>
        public static bool IsSameMonthAs(this DateTime subjectDate, DateTime dateToCompare)
        {
            var monthIsSame = subjectDate.Month == dateToCompare.Month;
            var yearIsSame = subjectDate.Year == dateToCompare.Year;
            return monthIsSame && yearIsSame;
        }
    }
}
