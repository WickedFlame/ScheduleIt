using System;

namespace ScheduleIt
{
    /// <summary>
    /// Defines a task that is executed in the background with the help of <see cref="TaskServer"/>
    /// </summary>
    public interface IBackgroundTask : IDisposable
    {
        /// <summary>
        /// Execute the task
        /// </summary>
        /// <param name="context"></param>
        void Execute(ExecutionContext context);
    }
}
