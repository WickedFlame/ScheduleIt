using System;

namespace ScheduleIt
{
    /// <summary>
    ///
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Clear all minutes, seconds and milliseconds of a <see cref="DateTime"/>
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static DateTime ClearMinutes(this DateTime current)
        {
            return current.AddMinutes(-1 * current.Minute)
                .AddSeconds(-1 * current.Second)
                .AddMilliseconds(-1 * current.Millisecond);
        }

        /// <summary>
        /// Clear all minutes, seconds and milliseconds of a <see cref="DateTime"/>
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static DateTime ClearSeconds(this DateTime current)
        {
            return current.AddSeconds(-1 * current.Second)
                .AddMilliseconds(-1 * current.Millisecond);
        }
    }
}
