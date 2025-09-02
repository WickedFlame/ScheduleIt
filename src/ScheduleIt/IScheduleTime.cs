using System;

namespace ScheduleIt
{
    /// <summary>
    ///
    /// </summary>
    public interface IScheduleTime
    {
        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        DateTime Calculate(DateTime last);

        /// <summary>
        /// Get the next scheduletime
        /// </summary>
        /// <returns></returns>
        IScheduleTime GetChild();

        /// <summary>
        /// Add a further scheduletime that is executed after the current
        /// </summary>
        /// <param name="time"></param>
        void SetChild(IScheduleTime time);
    }
}
