using System.Collections.Generic;

namespace ScheduleIt.Storage
{
    /// <summary>
    /// Store for taskt information of processed tasks
    /// </summary>
    public interface ITaskStore : IEnumerable<TaskEntity>
    {
         /// <summary>
         /// Add a new item of task information
         /// </summary>
         /// <param name="entity"></param>
        void Add(TaskEntity entity);
    }
}
