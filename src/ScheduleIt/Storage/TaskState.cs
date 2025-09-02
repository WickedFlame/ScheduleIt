
namespace ScheduleIt.Storage
{
    /// <summary>
    /// Define the state the task is in
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// Taskprocessing is started
        /// </summary>
        Started,

        /// <summary>
        /// Taskprocessing is completed
        /// </summary>
        Completed,

        /// <summary>
        /// Taskprocessing caused an error
        /// </summary>
        Error,

        /// <summary>
        /// Task was aborted
        /// </summary>
        Aborted
    }
}
