using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleIt
{
    /// <summary>
    /// List containing all <see cref="IBackgroundTask"/> that are running on the <see cref="ITaskServer"/>
    /// </summary>
    public class TaskList : IEnumerable<IBackgroundTask>
    {
        private readonly object _locker = new();
        private readonly List<IBackgroundTask> _tasks = new();

        /// <summary>
        /// Add a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="task"></param>
        public void Add(IBackgroundTask task)
        {
            lock (_locker)
            {
                _tasks.Add(task);
            }
        }

        /// <summary>
        /// Remove a <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="task"></param>
        public void Remove(IBackgroundTask task)
        {
            lock (_locker)
            {
                _tasks.Remove(task);
                task.Dispose();
            }
        }

        /// <summary>
        /// Check if the instance is contained
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool Contains(IBackgroundTask task)
        {
            lock (_locker)
            {
                return _tasks.Contains(task);
            }
        }

        /// <summary>
        /// Copy the list of tasks
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBackgroundTask> Copy()
        {
            lock (_locker)
            {
                return _tasks.ToList();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IBackgroundTask> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
