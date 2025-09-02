using ScheduleIt.TimeUnits;
using System;

namespace ScheduleIt
{
    /// <summary>
    /// Create a schedule at a certain time
    /// </summary>
    public class UnitSchedule : ScheduleBase, IScheduleTime
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="unit"></param>
        public UnitSchedule(ITimeUnit unit)
        {
            Unit = unit;
        }

        internal ITimeUnit Unit { get; }

        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        public DateTime Calculate(DateTime last)
        {
            return Unit.Calculate(last);
        }
    }
}
