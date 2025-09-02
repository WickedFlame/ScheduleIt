using ScheduleIt.Diagnostics;
using ScheduleIt.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScheduleIt
{
    /// <summary>
    /// Scheduler for planing and executing tasks
    /// </summary>
    public class Scheduler : IScheduler
    {
        private readonly List<TaskSchedule> _schedules = new List<TaskSchedule>();

        /// <summary>
        ///
        /// </summary>
        public Scheduler()
        {
            WaitHandle = new ManualResetEvent(false);
        }

        /// <summary>
        /// Waithandle for waiting for next execution
        /// </summary>
        public EventWaitHandle WaitHandle { get; }

        /// <summary>
        /// Defines if the scheduler is running
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Gets the list of <see cref="TaskSchedule"/> that are registered to the scheduler
        /// </summary>
        public IEnumerable<TaskSchedule> Schedules => _schedules;

        /// <summary>
        /// Enqueue a new <see cref="IBackgroundTask"/> in the scheduler
        /// </summary>
        /// <param name="task"></param>
        /// <param name="scheduler"></param>
        /// <param name="name"></param>
        public void Enqueue(Func<IBackgroundTask> task, Action<Schedule> scheduler, string name)
        {
            var schedule = new Schedule();
            scheduler(schedule);

            var next = schedule.Next == DateTime.MinValue ? "now" : schedule.Next.ToString("o");
            LoggerFactory.Logger.Write($"Schedule new Task {name} to execute at {next}", LogLevel.Debug, nameof(Scheduler), "Enqueue");

            lock (_schedules)
            {
                _schedules.Add(new TaskSchedule
                {
                    Name = name,
                    Task = task,
                    Schedule = schedule,
                    ScheduleState = ScheduleState.Scheduled
                });
                schedule.CalculateNext(DateTime.Now);

                WaitHandle.Set();
            }
        }

        /// <summary>
        /// Remove a task from the Scheduler. Tasks that are being executed are not aborted.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            var tasks = _schedules.Where(s => s.Name == name).ToList();
            foreach (var task in tasks)
            {
                task.IsRunning = false;
                task.ScheduleState = ScheduleState.Removed;

                _schedules.Remove(task);
            }
        }

        /// <summary>
        /// Get the scheduled Task if it is availiable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TaskSchedule GetScheduledTask(string name)
        {
            return _schedules.Find(s => s.Name == name && s.ScheduleState == ScheduleState.Scheduled);
        }

        /// <summary>
        /// Execute all scheduled Tasks in the background
        /// </summary>
        /// <param name="context"></param>
        public void Execute(ExecutionContext context)
        {
            if (IsRunning)
            {
                return;
            }

            context.GetLogger().Write("Start execution of Schduler", LogLevel.Debug, nameof(Scheduler), "Execute");

            IsRunning = true;

            try
            {
                // check if the first execution is in the future
                var next = GetNext();
                if(next == null)
                {
                    IsRunning = false;
                    return;
                }

                var time = next.Schedule.TimeToNext();
                if (time > TimeSpan.Zero)
                {
                    WaitHandle.WaitOne(time);
                }

                while (IsRunning)
                {
                    // end the loop if the cancellationtoken is canceled
                    context.CancellationToken.ThrowIfCancellationRequested();

                    WaitHandle.Reset();

                    try
                    {
                        next = GetNext();
                        if (next != null)
                        {
                            next.ScheduleState = ScheduleState.Running;

                            time = next.Schedule.TimeToNext();
                            if (time <= TimeSpan.Zero)
                            {
                                try
                                {
                                    context.GetLogger().Write($"Start execution of Task {next.Name}", LogLevel.Debug, nameof(Scheduler), "Execute");

                                    var server = context.GetTaskServer();
                                    if (server != null)
                                    {
                                        server.StartNew(next.Task.Invoke());
                                    }
                                    else
                                    {
                                        next.Task.Invoke().Execute(context);
                                    }

                                    // don't run again for at least
                                    var nextExecTime = next.Schedule.CalculateNext(DateTime.Now);
                                    if (nextExecTime == DateTime.MinValue)
                                    {
                                        next.IsRunning = false;
                                    }

                                    // set the task to rescheduled or completed
                                    next.ScheduleState = next.IsRunning ? ScheduleState.Scheduled : ScheduleState.Completed;
                                }
                                catch(OperationCanceledException)
                                {
                                    next.IsRunning = false;
                                    next.ScheduleState = ScheduleState.Failed;
                                }
                                catch (Exception inner)
                                {
                                    context.GetLogger().Write($"Fehler beim Ausführen des Tasks {next.Name}. Ausführung des Tasks wird abgebrochen.{Environment.NewLine}{inner}", LogLevel.Error, nameof(Scheduler), "Execute");
                                    next.IsRunning = false;
                                    next.ScheduleState = ScheduleState.Failed;
                                }
                            }
                        }

                        CleanDisabledTasks();

                        next = GetNext();
                        if(next == null)
                        {
                            IsRunning = false;
                            return;
                        }

                        time = next.Schedule.TimeToNext();
                        if (time > TimeSpan.Zero)
                        { 
                            context.GetLogger().Write($"Scheduler wartet {time} bis zur nächsten Ausführung", LogLevel.Debug, nameof(Scheduler), "Execute");
                            WaitHandle.WaitOne(time);
                        }
                    }
                    catch (Exception e)
                    {
                        context.GetLogger().Write($"Fehler beim Ausführen eines {nameof(IBackgroundTask)}. Scheduler wird beendet!{Environment.NewLine}{e}", LogLevel.Error, nameof(Scheduler), "Execute");
                        IsRunning = false;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// Get the next task based on the the scheduled time
        /// </summary>
        /// <returns></returns>
        private TaskSchedule GetNext()
        {
            lock (_schedules)
            {
                var tasks = _schedules.Where(s => s.IsRunning).OrderBy(s => s.Schedule.TimeToNext());
                return tasks.FirstOrDefault();
            }
        }

        private void CleanDisabledTasks()
        {
            lock (_schedules)
            {
                foreach(var dead in _schedules.Where(s => !s.IsRunning).ToList())
                {
                    _schedules.Remove(dead);
                }
            }
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
            IsRunning = false;
            WaitHandle.Set();
            lock (_schedules)
            {
                _schedules.Clear();
            }
        }
    }
}
