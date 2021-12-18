using System;

namespace SPDC.Common
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Set time of date to 00:00:00
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToStartOfTheDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// Set time of date to 23:59:59:997 <br/>
        /// EF6 does not support storage 23:59:59:999 or 23:59:59:998 with SQL Server
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToEndOfTheDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day).AddDays(1.0).AddMilliseconds(-2);
        }
    }
}
