using System;

namespace ScheduleIt.TimeUnits
{
    /// <summary>
    ///
    /// </summary>
    public class HourUnit : ITimeUnit
    {
        private readonly int _interval;
        private Func<DateTime, DateTime> _factory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        public HourUnit(int interval)
        {
            _interval = interval;
            _factory = d => d.AddHours(interval);
        }

        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        public DateTime Calculate(DateTime last)
        {
            return _factory(last);
        }

        /// <summary>
        /// Schedule at a certain minute of the hour
        /// </summary>
        /// <param name="minutes"></param>
        public void At(int minutes)
        {
            _factory = d => d.AddHours(_interval).ClearMinutes().Add(TimeSpan.FromMinutes(minutes));
        }
    }
}
