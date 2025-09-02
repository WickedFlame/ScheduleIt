using ScheduleIt.Diagnostics;
using System.Diagnostics;
using static ScheduleIt.Test.TaskServerTests;

namespace ScheduleIt.Test
{
    public class SchedulerTests
    {
        [Test]
        public void Scheduler_IsRunning()
        {
            var scheduler = new Scheduler();
            scheduler.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Scheduler_IsRunning_Execute()
        {
            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() =>
            {
                scheduler.IsRunning.Should().BeTrue();
            }), s => s.Now(), "test");

            scheduler.Execute(new ExecutionContext());
        }

        [Test]
        public void Scheduler_Execute_NoTask()
        {
            var scheduler = new Scheduler();
            scheduler.Execute(new ExecutionContext());

            scheduler.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Scheduler_Execute_Single()
        {
            var set = false;

            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() => set = true ), s => s.Now(), "test");

            scheduler.Execute(new ExecutionContext());

            set.Should().BeTrue();
        }

        [Test]
        public void Scheduler_Execute_MultipleTimes()
        {
            var cnt = 0;

            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() =>
            {
                cnt++;
                scheduler.IsRunning = cnt < 5;
            }), s => s.Now().AndEvery(TimeSpan.FromMilliseconds(5)), "test");

            scheduler.Execute(new ExecutionContext());

            Task.Delay(50).Wait();

            cnt.Should().Be(5);
        }

        [Test]
        public void Scheduler_Execute_Multiple_Single()
        {
            var cnt = 0;

            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() => cnt++ ), s => s.Now(), "test");
            scheduler.Enqueue(() => new TestTask(() => cnt++ ), s => s.Now(), "test 2");
            scheduler.Enqueue(() => new TestTask(() => cnt++ ), s => s.Now(), "test 3");

            scheduler.Execute(new ExecutionContext());

            Task.Delay(50).Wait();

            cnt.Should().Be(3);
        }

        [Test]
        public void Scheduler_Execute_FirstDelay()
        {
            var scheduler = new Scheduler();

            var sw = Stopwatch.StartNew();
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMilliseconds(50)), "test");
            scheduler.Execute(new ExecutionContext());
            sw.Stop();

            sw.ElapsedMilliseconds.Should().BeGreaterThanOrEqualTo(50).And.BeLessThan(70);
        }

        [Test]
        public void Scheduler_Execute_Exception()
        {
            var scheduler = new Scheduler();

            scheduler.Enqueue(() => new TestTask(() => throw new Exception("error")), s => s.Now(), "test");

            scheduler.Execute(new ExecutionContext());

            scheduler.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Scheduler_Execute_CancellationToken_Stop()
        {
            var scheduler = new Scheduler();

            var tokenSource = new CancellationTokenSource();
            var context = new ExecutionContext
            {
                CancellationToken = tokenSource.Token
            };
            scheduler.Enqueue(() => new TestTask(() => tokenSource.Cancel()), s => s.Now(), "test");

            scheduler.Execute(context);

            scheduler.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Scheduler_Execute_CancellationToken()
        {
            var scheduler = new Scheduler();

            var tokenSource = new CancellationTokenSource();
            var context = new ExecutionContext
            {
                CancellationToken = tokenSource.Token
            };
            tokenSource.Cancel();

            scheduler.Enqueue(() => new TestTask(() => { }), s => s.Every(1).Seconds(), "test");

            scheduler.Execute(context);

            scheduler.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Scheduler_Execute_Log_StartExecution()
        {
            var logger = new Mock<ILog>();
            var scheduler = new Scheduler();

            var context = new ExecutionContext();
            context.Set<ILog>(nameof(ILog), logger.Object);

            scheduler.Enqueue(() => new TestTask(() => { }), s => s.Now(), "test");

            scheduler.Execute(context);

            logger.Verify(x => x.Write("Start execution of Schduler", LogLevel.Debug, "Scheduler", "Execute"), Times.Once);
        }

        [Test]
        public void Scheduler_Execute_Log_StartExecutionTask()
        {
            var logger = new Mock<ILog>();
            var scheduler = new Scheduler();

            var context = new ExecutionContext();
            context.Set<ILog>(nameof(ILog), logger.Object);

            scheduler.Enqueue(() => new TestTask(() => { }), s => s.Now(), "test");

            scheduler.Execute(context);

            logger.Verify(x => x.Write("Start execution of Task test", LogLevel.Debug, "Scheduler", "Execute"), Times.Once);
        }

        [Test]
        public void Scheduler_Execute_Log_Error()
        {
            var logger = new Mock<ILog>();

            var scheduler = new Scheduler();

            var context = new ExecutionContext();
            context.Set<ILog>(nameof(ILog), logger.Object);

            scheduler.Enqueue(() => new TestTask(() => throw new Exception()), s => s.Now(), "test");

            scheduler.Execute(context);

            logger.Verify(x => x.Write(It.Is<string>(s => s.StartsWith("Fehler beim Ausführen des Tasks test")), LogLevel.Error, "Scheduler", "Execute"), Times.Once);
        }

        [Test]
        public void Scheduler_Enqueue_Log()
        {
            var logger = new Mock<ILog>();
            LoggerFactory.Logger = logger.Object;

            var scheduler = new Scheduler();

            scheduler.Enqueue(() => new TestTask(() => throw new Exception()), s => s.Now(), "test");

            logger.Verify(x => x.Write(It.Is<string>(s => s.StartsWith("Schedule new Task test to execute at")), LogLevel.Debug, "Scheduler", "Enqueue"), Times.Once);
        }

        [Test]
        public void Scheduler_Enqueue_Series_In()
        {
            var server = new TaskServer();
            server.Schedule(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(1)), "first");
            var monitor = new ScheduleIt.Diagnostics.Monitor(server);
            monitor.ScheduledTasks.Should().HaveCount(1);
            monitor.Completed.Should().HaveCount(0);

            server.Schedule(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(1)), "second");
            monitor = new ScheduleIt.Diagnostics.Monitor(server);
            monitor.ScheduledTasks.Should().HaveCount(2);
            monitor.Completed.Should().HaveCount(0);

            server.Schedule(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMilliseconds(10)), "third");

            Task.Delay(50).Wait();

            monitor = new ScheduleIt.Diagnostics.Monitor(server);
            monitor.ScheduledTasks.Should().HaveCount(2);
            monitor.Completed.Should().HaveCount(1);
        }

        [Test]
        public void Scheduler_Enqueue_Series_At()
        {
            var server = new TaskServer();
            server.Schedule(() => new TestTask(() => { }), s => s.At(DateTime.Now.AddMinutes(1)), "first");
            var monitor = new ScheduleIt.Diagnostics.Monitor(server);
            monitor.ScheduledTasks.Should().HaveCount(1);
            monitor.Completed.Should().HaveCount(0);

            server.Schedule(() => new TestTask(() => { }), s => s.At(DateTime.Now.AddMinutes(1)), "second");
            monitor = new ScheduleIt.Diagnostics.Monitor(server);
            monitor.ScheduledTasks.Should().HaveCount(2);
            monitor.Completed.Should().HaveCount(0);

            server.Schedule(() => new TestTask(() => { }), s => s.At(DateTime.Now.AddMilliseconds(10)), "third");

            Task.Delay(50).Wait();

            monitor = new ScheduleIt.Diagnostics.Monitor(server);
            monitor.ScheduledTasks.Should().HaveCount(2);
            monitor.Completed.Should().HaveCount(1);
        }

        [Test]
        public void Scheduler_Execute_TasksRemoved()
        {
            var scheduler = new Scheduler();
            var context = new ExecutionContext();

            scheduler.Enqueue(() => new TestTask(() => { }), s => s.Now(), "test");
            scheduler.Execute(context);

            scheduler.Schedules.Should().BeEmpty();
        }

        [Test]
        public void Scheduler_Execute_AbortExecution()
        {
            var scheduler = new Scheduler();
            var context = new ExecutionContext();

            scheduler.Enqueue(() => new AbortExecutionTask(), s => s.Every(TimeSpan.FromMilliseconds(50)), "test");
            scheduler.Execute(context);

            scheduler.Schedules.Should().BeEmpty();
        }






        [Test]
        public void Scheduler_Execute_In_Start()
        {
            var scheduler = new Scheduler();

            var context = new ExecutionContext();
            var start = DateTime.Now;
            var end = DateTime.MinValue;
            scheduler.Enqueue(() => new TestTask(() => { end = DateTime.Now; }), s => s.In(TimeSpan.FromMilliseconds(20)), "test");

            scheduler.Execute(context);

            end.Subtract(start).Should().BeGreaterThan(TimeSpan.FromMilliseconds(20));
        }

        [Test]
        public void Scheduler_Execute_Every_Start()
        {
            var scheduler = new Scheduler();

            var tokenSource = new CancellationTokenSource();
            var context = new ExecutionContext
            {
                CancellationToken = tokenSource.Token
            };

            var start = DateTime.Now;
            var end = DateTime.MinValue;
            scheduler.Enqueue(() => new TestTask(() =>
            {
                end = DateTime.Now;
                tokenSource.Cancel();
            }), s => s.Every(TimeSpan.FromMilliseconds(20)), "test");

            scheduler.Execute(context);

            end.Subtract(start).Should().BeGreaterThan(TimeSpan.FromMilliseconds(20));
        }

        [Test]
        public void Scheduler_Execute_At_Start()
        {
            var scheduler = new Scheduler();

            var context = new ExecutionContext();
            var start = DateTime.Now;
            var end = DateTime.MinValue;
            scheduler.Enqueue(() => new TestTask(() => { end = DateTime.Now; }), s => s.At(DateTime.Now.AddMilliseconds(200)), "test");

            scheduler.Execute(context);

            end.Subtract(start).Should().BeGreaterThan(TimeSpan.FromMilliseconds(200));
        }

        [Test]
        public void Scheduler_Remove()
        {
            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(10)), "test");

            scheduler.Remove("test");

            scheduler.Schedules.Should().BeEmpty();
        }

        [Test]
        public void Scheduler_Remove_Multiple()
        {
            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(10)), "test");
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(10)), "test");

            scheduler.Remove("test");

            scheduler.Schedules.Should().BeEmpty();
        }

        [Test]
        public void Scheduler_Remove_OnlyNamed()
        {
            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(10)), "test");
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(10)), "test");
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(10)), "stay");

            scheduler.Remove("test");

            scheduler.Schedules.Should().HaveCount(1);
        }

        [Test]
        public void Scheduler_GetScheduledTask()
        {
            var scheduler = new Scheduler();
            var task = () => new TestTask(() => { });
            scheduler.Enqueue(task, s => s.In(TimeSpan.FromMinutes(10)), "test");

            var scheduled = scheduler.GetScheduledTask("test");

            scheduled.Task.Should().BeSameAs(task);
            scheduled.Name.Should().Be("test");
        }

        [Test]
        public void Scheduler_GetScheduledTask_NotFound()
        {
            var scheduler = new Scheduler();

            scheduler.GetScheduledTask("test")
                .Should().BeNull();
        }

        [Test]
        public void Scheduler_GetScheduledTask_Completed()
        {
            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.Now(), "test");

            scheduler.Execute(new ExecutionContext());

            scheduler.GetScheduledTask("test").Should().BeNull();
        }

        [Test]
        public void Scheduler_GetScheduledTask_Completed_Task()
        {
            var scheduler = new Scheduler();
            scheduler.Enqueue(() => new TestTask(() => { }), s => s.Now(), "test");

            var scheduled = scheduler.GetScheduledTask("test");

            scheduler.Execute(new ExecutionContext());

            scheduled.ScheduleState.Should().Be(Scheduling.ScheduleState.Completed);
        }
    }
}
