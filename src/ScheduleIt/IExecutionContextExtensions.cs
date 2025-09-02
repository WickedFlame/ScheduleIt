using ScheduleIt.Diagnostics;
using System;

namespace ScheduleIt
{
    /// <summary>
    ///
    /// </summary>
    public static class IExecutionContextExtensions
    {
        /// <summary>
        /// Get the TaskServer if it is registered to the context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ITaskServer GetTaskServer(this IExecutionContext context)
        {
            return context.Get<TaskServer>(nameof(TaskServer));
        }

        /// <summary>
        /// Get the Logger that is registered to the context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILog GetLogger(this IExecutionContext context)
        {
            return context.Get<ILog>(nameof(ILog)) ?? new Logger();
        }

        /// <summary>
        /// Abort the execution of the task
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="OperationCanceledException"></exception>
        public static void AbortExecution(this IExecutionContext context)
        {
            throw new OperationCanceledException("End of operation requested");
        }

        /// <summary>
        /// Execute a task in the same thread
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        public static void ExecuteSubtask<T>(this ExecutionContext context) where T : class, IBackgroundTask
        {
            var server = context.Get<TaskServer>(nameof(TaskServer));
            var task = server.Resolver.Resolve<T>();
            task.Execute(context);
        }
    }
}
