using System;

namespace ScheduleIt
{
    /// <summary>
    /// Schedule time for a given <see cref="TimeSpan"/> interval
    /// </summary>
    public class ScheduleTimeSpan : ScheduleBase, IScheduleTime
    {
        private readonly TimeSpan _interval;

        /// <summary>
        /// Schedule time for a given <see cref="TimeSpan"/> interval
        /// </summary>
        /// <param name="interval"></param>
        public ScheduleTimeSpan(TimeSpan interval)
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
            return last.Add(_interval);
        }
    }
}
