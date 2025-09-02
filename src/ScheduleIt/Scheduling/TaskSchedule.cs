using ScheduleIt.Storage;
using System;
using System.Diagnostics;

namespace ScheduleIt.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    [DebuggerDisplay("Name = {Name}, {Schedule}")]
    public class TaskSchedule
    {
        /// <summary>
        /// Gets or sets the Name of the Task
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the Task that is executed on the scheduler
        /// </summary>
        public Func<IBackgroundTask> Task { get; internal set; }

        /// <summary>
        /// Gets the schedule that defines when the task is executed
        /// </summary>
        public Schedule Schedule { get; internal set; }

        /// <summary>
        /// Defines if the task is still run in the scheduler
        /// </summary>
        public bool IsRunning { get; set; } = true;

        /// <summary>
        /// Gets the state that the scheduled task is in
        /// </summary>
        public ScheduleState ScheduleState { get; internal set; }
    }
}
