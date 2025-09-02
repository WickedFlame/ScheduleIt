using System;

namespace ScheduleIt.Templates
{
    /// <summary>
    /// Basic task that executes a <see cref="Action"/>
    /// </summary>
    public class BasicTask : IBackgroundTask
    {
        private readonly Action _action;

        /// <summary>
        /// Create a new basic task containing the <see cref="Action"/> to execute
        /// </summary>
        /// <param name="action"></param>
        public BasicTask(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// Execute the <see cref="Action"/>
        /// </summary>
        /// <param name="context"></param>
        public void Execute(ExecutionContext context)
        {
            _action();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
