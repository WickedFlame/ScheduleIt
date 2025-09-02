using ScheduleIt.Diagnostics;
using System;
using System.Linq;

namespace ScheduleIt
{
    /// <summary>
    ///
    /// </summary>
    public static class TaskServerExtensions
    {
        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        public static ITaskServer Schedule(this ITaskServer server, IBackgroundTask task, Action<Schedule> scheduler)
        {
            server.Schedule(() => task, scheduler, task.GetType().Name);

            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="server"></param>
        /// <param name="scheduler"></param>
        public static ITaskServer Schedule<T>(this ITaskServer server, Action<Schedule> scheduler) where T : class, IBackgroundTask
        {
            server.Schedule(() => server.Resolver.Resolve<T>(), scheduler, typeof(T).Name);
            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="server"></param>
        /// <param name="scheduler"></param>
        /// <param name="name"></param>
        public static ITaskServer Schedule<T>(this ITaskServer server, Action<Schedule> scheduler, string name) where T : class, IBackgroundTask
        {
            server.Schedule(() => server.Resolver.Resolve<T>(), scheduler, name);
            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        public static ITaskServer Schedule(this ITaskServer server, Func<IBackgroundTask> task, Action<Schedule> scheduler)
        {
            server.Schedule(task, scheduler, Guid.NewGuid().ToString());
            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        public static ITaskServer Schedule(this ITaskServer server, Action task, Action<Schedule> scheduler)
        {
            server.Schedule(task, scheduler, Guid.NewGuid().ToString());
            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        public static ITaskServer Schedule(this ITaskServer server, Action<ExecutionContext> task, Action<Schedule> scheduler)
        {
            server.Schedule(task, scheduler, Guid.NewGuid().ToString());
            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        /// <param name="name"></param>
        public static ITaskServer Schedule(this ITaskServer server, Action task, Action<Schedule> scheduler, string name)
        {
            server.Schedule(() => new Templates.BasicTask(task), scheduler, name);
            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        /// <param name="name"></param>
        public static ITaskServer Schedule(this ITaskServer server, Action<ExecutionContext> task, Action<Schedule> scheduler, string name)
        {
            server.Schedule(() => new Templates.ContextTask(task), scheduler, name);
            return server;
        }

        /// <summary>
        /// Schedule a new <see cref="IBackgroundTask"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        /// <param name="name"></param>
        public static ITaskServer Schedule(this ITaskServer server, Func<IBackgroundTask> task, Action<Schedule> scheduler, string name)
        {
            server.Scheduler.Enqueue(task, scheduler, name);
            if (!server.Scheduler.IsRunning && !server.Tasks.Contains(server.Scheduler))
            {
                server.StartNew(server.Scheduler);
            }

            return server;
        }

        /// <summary>
        /// Execute a <see cref="IBackgroundTask"/> in a new Thread
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="server"></param>
        /// <returns></returns>
        public static ITaskServer StartNew<T>(this ITaskServer server) where T : class, IBackgroundTask
        {
            server.StartNew(server.Resolver.Resolve<T>());
            return server;
        }

        /// <summary>
        /// Remove the scheduled task. Running tasks will not be stoped
        /// </summary>
        /// <param name="server"></param>
        /// <param name="name"></param>
        public static void Remove(this ITaskServer server, string name)
        {
            server.Scheduler.Remove(name);
        }

        /// <summary>
        /// Remove the scheduled task. Running tasks will not be stoped
        /// </summary>
        /// <param name="server"></param>
        public static void Remove<T>(this ITaskServer server) where T : class, IBackgroundTask
        {
            server.Scheduler.Remove(typeof(T).Name);
        }

        /// <summary>
        /// Set the current TaskServer as the default instance
        /// </summary>
        /// <param name="server"></param>
        public static ITaskServer SetDefault(this ITaskServer server)
        {
            TaskServer.Instance = server;

            return server;
        }

        /// <summary>
        /// Get the Monitorobject containing information on all running tasks and schedules
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public static Monitor GetMonitor(this ITaskServer server)
        {
            return new Monitor(server);
        }

        /// <summary>
        /// Checks if the task is scheduled for execution
        /// </summary>
        /// <param name="server"></param>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public static bool IsTaskScheduled(this ITaskServer server, string taskName)
        {
            return server.Scheduler.GetScheduledTask(taskName) != null;
        }
    }
}
