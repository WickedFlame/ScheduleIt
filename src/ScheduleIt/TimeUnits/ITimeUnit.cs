using System;

namespace ScheduleIt.TimeUnits
{
    /// <summary>
    ///
    /// </summary>
    public interface ITimeUnit
    {
        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        DateTime Calculate(DateTime last);
    }
}
