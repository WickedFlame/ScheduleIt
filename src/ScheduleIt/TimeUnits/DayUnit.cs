using System;

namespace ScheduleIt.TimeUnits
{
    /// <summary>
    ///
    /// </summary>
    public class DayUnit : ITimeUnit
    {
        private readonly int _interval;
        private Func<DateTime, DateTime> _factory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        public DayUnit(int interval)
        {
            _interval = interval;
            _factory = d => d.AddDays(interval);
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
        /// Schedule at a certain hour of the day
        /// </summary>
        /// <param name="hour"></param>
        public void At(int hour)
        {
            _factory = d =>
            {
                var next = d.Date.AddHours(hour).ClearMinutes();
                return next > d ? next : next.AddDays(_interval);
            };
        }

        /// <summary>
        /// Schedule at a certain hour and minute of the day
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minutes"></param>
        public void At(int hour, int minutes)
        {
            _factory = d =>
            {
                var next = d.Date.AddHours(hour).AddMinutes(minutes);
                return next > d ? next : next.AddDays(_interval);
            };
        }
    }
}
