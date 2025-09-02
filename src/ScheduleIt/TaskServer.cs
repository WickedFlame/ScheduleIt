using ScheduleIt.Diagnostics;
using ScheduleIt.IoC;
using ScheduleIt.Storage;
using System;
using System.Diagnostics;
using System.Threading;

namespace ScheduleIt
{
    /// <summary>
    /// Server for scheduling and executing tasks in the background in a own thread
    /// </summary>
    public class TaskServer : ITaskServer
    {
        /// <summary>
        /// The default instance of the taskserver
        /// </summary>
        public static ITaskServer Instance { get; internal set; } = new TaskServer();

        private readonly TaskStart _taskStart;

        /// <summary>
        /// Server for scheduling and executing tasks in the background in a own thread
        /// </summary>
        public TaskServer()
        {
            _taskStart = new TaskStart();
            Store = new TaskStore(new StoreOptions());

            Tasks = new TaskList();
        }

        /// <summary>
        /// Gets a list of all running tasks
        /// </summary>
        public TaskList Tasks { get; }

        /// <summary>
        /// Gets the activation container for resolving objects
        /// </summary>
        public IActivationContainer Resolver { get; set; } = new BasicActivationContainer();

        /// <summary>
        /// Gets or sets the scheduler for scheduling tasks on the TaskServer
        /// </summary>
        public IScheduler Scheduler { get; set; } = new Scheduler();

        /// <summary>
        /// Gets the associated <see cref="ITaskStore"/> that contains
        /// </summary>
        public ITaskStore Store { get; set; }

        /// <summary>
        /// Execute a <see cref="IBackgroundTask"/> in a new thread
        /// </summary>
        /// <param name="task"></param>
        public void StartNew(IBackgroundTask task)
        {
            Tasks.Add(task);

            var context = new ExecutionContext
            {
                CancellationToken = _taskStart.CancellationToken
            };

            context.Set(nameof(TaskServer), this);
            context.Set(nameof(ILog), LoggerFactory.Logger);
            context.Set(nameof(ITaskStore), Store);

            _taskStart.StartNew(() =>
            {
                if(Thread.CurrentThread.Name == null)
                {
                    Thread.CurrentThread.Name = $"TaskServer - {task.GetType().Name}";
                }

                var store = context.Get<ITaskStore>(nameof(ITaskStore));
                var entity = new TaskEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    State = TaskState.Started,
                    StartTime = DateTime.Now,
                    TaskType = task.GetType().Name
                };

                if (!(task is Scheduler))
                {
                    // do not track the Scheduler Task
                    // only track tasks executed by the using application
                    store.Add(entity);
                }

                var sw = Stopwatch.StartNew();
                try
                {
                    task.Execute(context);
                    entity.State = TaskState.Completed;
                }
                catch (OperationCanceledException)
                {
                    entity.State = TaskState.Aborted;
                }
                catch (Exception e)
                {
                    entity.State = TaskState.Error;
                    context.GetLogger().Write($"Fehler beim Ausführen eines {nameof(IBackgroundTask)}. Ausführung wird beendet!{Environment.NewLine}{e}", LogLevel.Error, "TaskServer", "StartNew");
                }

                sw.Stop();
                entity.Duration = sw.Elapsed;

                context.GetLogger().Write($"Task {entity.TaskType} execution took {entity.Duration.TotalMilliseconds} ms", LogLevel.Information, "TaskServer", "StartNew");

                Tasks.Remove(task);
            });
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
            foreach (var task in Tasks.Copy())
            {
                task.Dispose();
            }

            _taskStart.Dispose();
        }

        /// <summary>
        /// Setup the default TaskServer
        /// </summary>
        /// <param name="factory"></param>
        public static ITaskServer Setup(Action<TaskServerBuilder> factory)
        {
            var builder = new TaskServerBuilder();
            factory(builder);

            return builder.Build();
        }
    }
}
