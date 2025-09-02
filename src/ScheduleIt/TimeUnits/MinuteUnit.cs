using System;

namespace ScheduleIt.TimeUnits
{
    /// <summary>
    ///
    /// </summary>
    public class MinuteUnit : ITimeUnit
    {
        private readonly int _interval;

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        public MinuteUnit(int interval)
        {
            _interval = interval;
        }

        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        public DateTime Calculate(DateTime last)
        {
            return last.AddMinutes(_interval);
        }
    }
}
