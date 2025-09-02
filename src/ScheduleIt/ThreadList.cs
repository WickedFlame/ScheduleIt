using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ScheduleIt
{
    /// <summary>
    /// A list containing all Threads that are currently running/registered
    /// </summary>
    public class ThreadList : IEnumerable<Task>, IDisposable
    {
        private readonly List<Task> _tasks = new List<Task>();
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// A list containing all Threads that are currently running/registered
        /// </summary>
        public ThreadList()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Add a new Thread. Threads are automatically removed when they complete
        /// </summary>
        /// <param name="thread"></param>
        public void Add(Task thread)
        {
            _tasks.Add(thread);
            thread.ContinueWith(t => _tasks.Remove(t));
        }

        /// <summary>
        ///
        /// </summary>
        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Task> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///
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
            //
            // Cancel the Tasks with the CancellationToken
            // Do not call Dispose() on the Tasks. This causes an Exception that is not handled
            // https://devblogs.microsoft.com/pfxteam/do-i-need-to-dispose-of-tasks/
            //

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
