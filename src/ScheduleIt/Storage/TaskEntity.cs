using System;

namespace ScheduleIt.Storage
{
    /// <summary>
    ///
    /// </summary>
    public class TaskEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public TaskState State { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Name of the Typr of Task
        /// </summary>
        public string TaskType { get; set; }
    }
}
