using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScheduleIt
{
    /// <summary>
    /// Start a task in a new Thread
    /// </summary>
    public class TaskStart : ITaskStart
    {
        private readonly ThreadList _threads = new ThreadList();

        /// <summary>
        /// Start a task in a new Thread
        /// </summary>
        /// <param name="task"></param>
        public void StartNew(Action task)
        {
            var thread = Task.Factory.StartNew(task, CancellationToken, TaskCreationOptions.AttachedToParent, System.Threading.Tasks.TaskScheduler.Default);
            _threads.Add(thread);
        }

        /// <summary>
        /// Gets the CancellationToken to stop the taks
        /// </summary>
        public CancellationToken CancellationToken => _threads.CancellationToken;

        /// <summary>
        /// List of all threads
        /// </summary>
        public ThreadList Threads => _threads;

        /// <summary>
        /// Dispose the list of threads
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            _threads.Dispose();
        }
    }
}
