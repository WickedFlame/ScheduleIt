using ScheduleIt.Scheduling;
using System;
using System.Collections.Generic;

namespace ScheduleIt
{
    /// <summary>
    /// Scheduler for planing and executing tasks
    /// </summary>
    public interface IScheduler : IBackgroundTask
    {
        /// <summary>
        /// Defines if the scheduler is running
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// Gets the list of <see cref="TaskSchedule"/> that are registered to the scheduler
        /// </summary>
        IEnumerable<TaskSchedule> Schedules { get; }

        /// <summary>
        /// Enqueue a new <see cref="IBackgroundTask"/> in the scheduler
        /// </summary>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        /// <param name="name"></param>
        void Enqueue(Func<IBackgroundTask> task, Action<Schedule> scheduler, string name);

        /// <summary>
        /// Remove a task from the Scheduler. Tasks that are being executed are not aborted.
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);

        /// <summary>
        /// Get the scheduled Task if it is availiable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TaskSchedule GetScheduledTask(string name);
    }
}
