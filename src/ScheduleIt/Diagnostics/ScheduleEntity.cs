using System;

namespace ScheduleIt.Diagnostics
{
    /// <summary>
    /// Represents a scheduled task
    /// </summary>
    public class ScheduleEntity
    {
        /// <summary>
        /// Name of the scheduled task
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets if the schedule is active
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Gets the next execution of the schedule
        /// </summary>
        public DateTime Next { get; set; }
    }
}
