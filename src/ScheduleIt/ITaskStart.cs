using System;

namespace ScheduleIt
{
    /// <summary>
    /// Start a task in a new Thread
    /// </summary>
    public interface ITaskStart : IDisposable
    {
        /// <summary>
        /// Start a task in a new Thread
        /// </summary>
        /// <param name="task"></param>
        void StartNew(Action task);
    }
}
