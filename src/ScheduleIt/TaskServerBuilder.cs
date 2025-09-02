using ScheduleIt.Diagnostics;
using ScheduleIt.IoC;
using ScheduleIt.Storage;
using System;
using System.Collections.Generic;

namespace ScheduleIt
{
    /// <summary>
    /// BUilder used to create a TaskServer. This is initiated through <see cref="TaskServer.Setup"/>
    /// </summary>
    public class TaskServerBuilder
    {
        private ITaskServer _server = new TaskServer();

        /// <summary>
        ///
        /// </summary>
        public TaskServerBuilder()
        {
            ServerTasks = new List<Action<ITaskServer>>();
        }

        /// <summary>
        /// List of actions that the builder uses to create the taskserver
        /// </summary>
        public List<Action<ITaskServer>> ServerTasks { get; }

        public TaskServerBuilder SetServer(ITaskServer server)
        {
            _server = server;
            return this;
        }

        /// <summary>
        /// Build the taskserver
        /// </summary>
        /// <returns></returns>
        public ITaskServer Build()
        {
            var server = _server ?? new TaskServer();

            foreach(var task in ServerTasks)
            {
                task(server);
            }

            return server;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static class TaskServerBuilderExtensions
    {
        /// <summary>
        /// Define a custom <see cref="IActivationContainer"/> that is used by the taskserver
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static TaskServerBuilder UseActivationContainer(this TaskServerBuilder builder, IActivationContainer resolver)
        {
            builder.ServerTasks.Add(s => s.Resolver = resolver);

            return builder;
        }

        /// <summary>
        /// Set the default LogWriter
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static TaskServerBuilder UseLogger(this TaskServerBuilder builder, LogWriter logger)
        {
            LoggerFactory.DefaultWriter = logger;

            return builder;
        }

        /// <summary>
        /// Set the default LogWriter
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static TaskServerBuilder UseLogger(this TaskServerBuilder builder, ILog logger)
        {
            LoggerFactory.Logger = logger;

            return builder;
        }

        /// <summary>
        /// Set options that are used for the TaskStore
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TaskServerBuilder UseStoreOptions(this TaskServerBuilder builder, StoreOptions options)
        {
            builder.ServerTasks.Add(s => s.Store = new TaskStore(options));

            return builder;
        }

        public static TaskServerBuilder UseScheduler(this TaskServerBuilder builder, IScheduler scheduler)
        {
            builder.ServerTasks.Add(s => s.Scheduler = scheduler);

            return builder;
        }
    }
}
