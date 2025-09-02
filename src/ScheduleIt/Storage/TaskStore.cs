using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleIt.Storage
{
    /// <summary>
    /// Store for taskt information of processed tasks
    /// </summary>
    public class TaskStore : ITaskStore
    {
        private readonly object _lock = new();

        private readonly List<TaskEntity> _entities = new List<TaskEntity>();
        private readonly ITaskStart _taskStart;

        private bool _shrinking;

        /// <summary>
        /// Store for taskt information of processed tasks
        /// </summary>
        /// <param name="options"></param>
        public TaskStore(StoreOptions options)
            : this(options, new TaskStart())
        {
        }

        /// <summary>
        /// Store for taskt information of processed tasks
        /// </summary>
        /// <param name="options"></param>
        /// <param name="taskStart"></param>
        public TaskStore(StoreOptions options, ITaskStart taskStart)
        {
            _taskStart = taskStart;
            Options = options;
        }

        /// <summary>
        /// Gets the StoreOptions
        /// </summary>
        public StoreOptions Options { get; set; }

        /// <summary>
        /// Add a new item of task information
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TaskEntity entity)
        {
            lock (_lock)
            {
                _entities.Add(entity);
            }

            if (_entities.Count > Options.MaxEntries)
            {
                if (_shrinking)
                {
                    return;
                }
                _shrinking = true;
                _taskStart.StartNew(() => ShrinkStore(_entities, Options.ShrinkCount));
            }
        }

        private void ShrinkStore(List<TaskEntity> entities, int count)
        {
            lock (_lock)
            {
                var old = entities.OrderBy(e => e.StartTime).Take(count).ToList();
                foreach (var entity in old)
                {
                    entities.Remove(entity);
                }

                _shrinking = false;
            }
        }

        /// <summary>
        /// Get the enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TaskEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        /// <summary>
        /// Get the enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
