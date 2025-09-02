using ScheduleIt.Storage;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleIt.Diagnostics
{
    /// <summary>
    /// Gets a monitor object containing some information on the schdeuled and processing tasks of the server
    /// </summary>
    public class Monitor
    {
        /// <summary>
        /// Gets a monitor object containing some information on the schdeuled and processing tasks of the server
        /// </summary>
        /// <param name="server"></param>
        public Monitor(ITaskServer server)
        {
            ScheduledTasks = server.Scheduler.Schedules.Select(s => new ScheduleEntity
            {
                Name = s.Name,
                IsRunning = s.IsRunning,
                Next = s.Schedule.Next
            }).ToList();
            Running = server.Store.Where(s => s.State == TaskState.Started).ToList();
            Completed = server.Store.Where(s => s.State == TaskState.Completed).ToList();
        }

        /// <summary>
        /// Gets a list of objects representing all scheduled tasks
        /// </summary>
        public IEnumerable<ScheduleEntity> ScheduledTasks { get; }

        /// <summary>
        /// Gets a list of all currently running tasks
        /// </summary>
        public IEnumerable<TaskEntity> Running { get; }

        /// <summary>
        /// Gets a list of all completed tasks
        /// </summary>
        public IEnumerable<TaskEntity> Completed { get; }
    }
}
