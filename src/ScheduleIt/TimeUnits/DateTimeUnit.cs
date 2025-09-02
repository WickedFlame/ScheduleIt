using System;

namespace ScheduleIt.TimeUnits
{
    /// <summary>
    /// Unit that indicates a schedule that is now
    /// </summary>
    public class DateTimeUnit : ITimeUnit
    {
        private readonly Schedule _schedule;
        private readonly DateTime _time;

        /// <summary>
        ///
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="time"></param>
        public DateTimeUnit(Schedule schedule, DateTime time)
        {
            _schedule = schedule;
            _time = time;
        }

        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public DateTime Calculate(DateTime last)
        {
            if (_time < last)
            {
                return DateTime.MinValue;
            }

            return _time;
        }

        /// <summary>
        /// Create a <see cref="IScheduleTime"/> that calculates the next schedule based on a given interval
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public ScheduleTime AndEvery(int interval)
        {
            return _schedule.Every(interval);
        }

        /// <summary>
        /// Create a <see cref="IScheduleTime"/> that calculates the next schedule based on a given <see cref="TimeSpan"/> interval
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public IScheduleTime AndEvery(TimeSpan interval)
        {
            return _schedule.Every(interval);
        }
    }
}
