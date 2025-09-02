using ScheduleIt.TimeUnits;
using System;

namespace ScheduleIt
{
    /// <summary>
    /// Schedule time for fluent configuration of schedules
    /// </summary>
    public class ScheduleTime : ScheduleBase, IScheduleTime
    {
        private readonly int _interval;

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        public ScheduleTime(int interval)
        {
            _interval = interval;
            Unit = new SecondUnit(interval);
        }

        internal ITimeUnit Unit { get; set; }

        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        public DateTime Calculate(DateTime last)
        {
            return Unit.Calculate(last);
        }

        /// <summary>
        /// Schedule for a seconds interval
        /// </summary>
        /// <returns></returns>
        public SecondUnit Seconds()
        {
            var unit = new SecondUnit(_interval);
            Unit = unit;
            return unit;
        }

        /// <summary>
        /// Schedule for a minute interval
        /// </summary>
        /// <returns></returns>
        public MinuteUnit Minutes()
        {
            var unit = new MinuteUnit(_interval);
            Unit = unit;
            return unit;
        }

        /// <summary>
        /// Schedule for a hour interval
        /// </summary>
        /// <returns></returns>
        public HourUnit Hours()
        {
            var unit = new HourUnit(_interval);
            Unit = unit;
            return unit;
        }

        /// <summary>
        /// Schedule for a day interval
        /// </summary>
        /// <returns></returns>
        public DayUnit Days()
        {
            var unit = new DayUnit(_interval);
            Unit = unit;
            return unit;
        }
    }
}
