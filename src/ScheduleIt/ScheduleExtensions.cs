using ScheduleIt.TimeUnits;
using System;

namespace ScheduleIt
{
    /// <summary>
    /// Extensionmethods for <see cref="Schedule"/>
    /// </summary>
    public static class ScheduleExtensions
    {
        /// <summary>
        /// Create a <see cref="ScheduleTime"/> that calculates the next schedule based on a unit of 1. Allows further configuration with the returned <see cref="ScheduleTime"/>
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static ScheduleTime Every(this Schedule schedule)
            => schedule.Every(1);

        /// <summary>
        /// Create a <see cref="ScheduleTime"/> that calculates the next schedule based on a given int. Allows further configuration with the returned <see cref="ScheduleTime"/>
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static ScheduleTime Every(this Schedule schedule, int interval)
        {
            var time = new ScheduleTime(interval);
            schedule.SetTime(time);

            return time;
        }

        /// <summary>
        /// Create a <see cref="IScheduleTime"/> that calculates the next schedule based on a given <see cref="TimeSpan"/> interval
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static IScheduleTime Every(this Schedule schedule, TimeSpan interval)
        {
            var time = new ScheduleTimeSpan(interval);
            schedule.SetTime(time);

            return time;
        }

        /// <summary>
        /// Create a <see cref="IScheduleTime"/> for a certain time of a date
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="scheduleTime"></param>
        /// <returns></returns>
        public static DateTimeUnit At(this Schedule schedule, DateTime scheduleTime)
        {
            var unit = new DateTimeUnit(schedule, scheduleTime);
            var time = new UnitSchedule(unit);

            schedule.SetTime(time);
            schedule.CalculateNext(DateTime.Now);

            return unit;
        }

        /// <summary>
        /// Create a <see cref="IScheduleTime"/> in the given time of the timespan
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static DateTimeUnit In(this Schedule schedule, TimeSpan timeSpan)
            => At(schedule, DateTime.Now.Add(timeSpan));

        /// <summary>
        /// Schedule now
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public static DateTimeUnit Now(this Schedule schedule)
        {
            schedule.Next = DateTime.Now;
            var unit = new DateTimeUnit(schedule, DateTime.Now);
            var time = new UnitSchedule(unit);

            schedule.SetTime(time);

            return unit;
        }

        /// <summary>
        /// Calculate the <see cref="TimeSpan"/> to the next scheduled time
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public static TimeSpan TimeToNext(this Schedule schedule)
        {
            if (schedule.Next == DateTime.MinValue)
            {
                return TimeSpan.Zero;
            }

            return schedule.Next.Subtract(DateTime.Now);
        }
    }
}
