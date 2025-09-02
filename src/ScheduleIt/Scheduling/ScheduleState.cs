namespace ScheduleIt.Scheduling
{
    /// <summary>
    /// 
    /// </summary>
    public enum ScheduleState
    {
        /// <summary>
        /// The Task is scheduled for execution
        /// </summary>
        Scheduled,

        /// <summary>
        /// The task is being executed
        /// </summary>
        Running,

        /// <summary>
        /// The scheduler has completed
        /// </summary>
        Completed,

        /// <summary>
        /// The schedule was removed from the execution
        /// </summary>
        Removed,

        /// <summary>
        /// 
        /// </summary>
        Failed
    }
}
