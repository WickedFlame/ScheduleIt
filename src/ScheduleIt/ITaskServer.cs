using ScheduleIt.IoC;
using ScheduleIt.Storage;
using System;

namespace ScheduleIt
{
    /// <summary>
    ///
    /// </summary>
    public interface ITaskServer : IDisposable
    {
        /// <summary>
        /// Gets the activation container for resolving objects
        /// </summary>
        IActivationContainer Resolver { get; set; }

        /// <summary>
        /// Gets or sets the scheduler for scheduling tasks on the TaskServer
        /// </summary>
        IScheduler Scheduler { get; set; }

        /// <summary>
        /// Gets a list of all running tasks
        /// </summary>
        TaskList Tasks { get; }

        /// <summary>
        /// Gets the associated <see cref="ITaskStore"/>
        /// </summary>
        ITaskStore Store { get; set; }

        /// <summary>
        /// Execute a <see cref="IBackgroundTask"/> in a new thread
        /// </summary>
        /// <param name="task"></param>
        void StartNew(IBackgroundTask task);
    }
}
