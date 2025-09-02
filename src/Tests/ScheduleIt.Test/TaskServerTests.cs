using ScheduleIt.IoC;
using ScheduleIt.Storage;

namespace ScheduleIt.Test
{
    [SingleThreaded]
    public class TaskServerTests
    {
        [Test]
        public void TaskServer_ctor()
        {
            Action act = () => new TaskServer();
            act.Should().NotThrow();
        }

        [Test]
        public void TaskServer_StartNew_Tasks()
        {
            var running = true;
            var task = new TestTask(() =>
            {
                while (running)
                {
                    Task.Delay(50).Wait();
                }
            });

            using var server = new TaskServer();
            server.StartNew(task);

            server.Tasks.Should().HaveCount(1).And.Contain(task);

            running = false;
        }

        [Test]
        public void TaskServer_StartNew_Generic()
        {
            var resolver = new Mock<IActivationContainer>();
            resolver.Setup(x => x.Resolve<SimpleTask>()).Returns(() => new SimpleTask());

            var server = new Mock<ITaskServer>();
            server.Setup(x => x.Resolver).Returns(() => resolver.Object);

            server.Object.StartNew<SimpleTask>();

            server.Verify(x => x.StartNew(It.IsAny<SimpleTask>()));
        }

        [Test]
        public void TaskServer_Remove_Name()
        {
            using var server = new TaskServer();
            server.Schedule(() => { }, s => s.In(TimeSpan.FromMinutes(5)), "test");

            server.Remove("test");

            server.Scheduler.Schedules.Should().BeEmpty();
        }

        [Test]
        public void TaskServer_Remove_Generic()
        {
            using var server = new TaskServer();
            server.Schedule<AbortExecutionTask>(s => s.In(TimeSpan.FromMinutes(5)));

            server.Remove<AbortExecutionTask>();

            server.Scheduler.Schedules.Should().BeEmpty();
        }

        [Test]
        public void TaskServer_Setup_ActivationContainer()
        {
            var container = new Mock<IActivationContainer>();
            using (var server = TaskServer.Setup(b => b.UseActivationContainer(container.Object)))
            {
                server.Resolver.Should().Be(container.Object);
            }
        }

        [Test]
        public void TaskServer_Setup_StoreOptions()
        {
            var options = new Storage.StoreOptions { MaxEntries = 10, ShrinkCount = 5 };
            using (var server = TaskServer.Setup(b => b.UseStoreOptions(options)))
            {
                ((TaskStore)server.Store).Options.Should().Be(options);
            }
        }

        [Test]
        public void TaskServer_Setup_StoreOptions_Values()
        {
            using (var server = TaskServer.Setup(b => b.UseStoreOptions(new Storage.StoreOptions { MaxEntries = 10, ShrinkCount = 5 })))
            {
                ((TaskStore)server.Store).Options.MaxEntries.Should().Be(10);
                ((TaskStore)server.Store).Options.ShrinkCount.Should().Be(5);
            }
        }

        [Test]
        public void TaskServer_Schedule_Action()
        {
            using var server = new TaskServer();
            server.Schedule(() => { }, s => s.In(TimeSpan.FromMinutes(5)));

            server.Scheduler.Schedules.Should().HaveCount(1);
        }

        [Test]
        public void TaskServer_Schedule_Action_Context()
        {
            using var server = new TaskServer();
            server.Schedule(c => { }, s => s.In(TimeSpan.FromMinutes(5)));

            server.Scheduler.Schedules.Should().HaveCount(1);
        }

        [Test]
        public void TaskServer_Schedule_Action_Context_Name()
        {
            using var server = new TaskServer();
            server.Schedule(c => { }, s => s.In(TimeSpan.FromMinutes(5)), "task");

            server.Scheduler.Schedules.Should().HaveCount(1);
        }

        [Test]
        public void TaskServer_Schedule_Func()
        {
            using var server = new TaskServer();
            server.Schedule(() => new SimpleTask(), s => s.In(TimeSpan.FromMinutes(5)));

            server.Scheduler.Schedules.Should().HaveCount(1);
        }

        [Test]
        public void TaskServer_Schedule_Func_Name()
        {
            using var server = new TaskServer();
            server.Schedule(() => new SimpleTask(), s => s.In(TimeSpan.FromMinutes(5)));

            server.Scheduler.Schedules.Single().Name.Should().NotBeEmpty();
        }

        [Test]
        public void TaskServer_Schedule_Generic()
        {
            using var server = new TaskServer();
            server.Schedule<SimpleTask>(s => s.In(TimeSpan.FromMinutes(5)));

            server.Scheduler.Schedules.Should().HaveCount(1);
        }

        [Test]
        public void TaskServer_Schedule_Generic_Name()
        {
            using var server = new TaskServer();
            server.Schedule<SimpleTask>(s => s.In(TimeSpan.FromMinutes(5)));

            server.Scheduler.Schedules.Single().Name.Should().Be(typeof(SimpleTask).Name);
        }

        [Test]
        public void TaskServer_Schedule_Generic_WithName()
        {
            using var server = new TaskServer();
            server.Schedule<SimpleTask>(s => s.In(TimeSpan.FromMinutes(5)), "test");

            server.Scheduler.Schedules.Should().HaveCount(1);
        }

        [Test]
        public void TaskServer_Schedule_Generic_WithName_Name()
        {
            using var server = new TaskServer();
            server.Schedule<SimpleTask>(s => s.In(TimeSpan.FromMinutes(5)), "test");

            server.Scheduler.Schedules.Single().Name.Should().Be("test");
        }

        [Test]
        public void TaskServer_Schedule_Daily_Past()
        {
            using var server = new TaskServer();
            server.Schedule<SimpleTask>(s => s.Every().Days().At(DateTime.Now.Hour -1), "test");

            server.Scheduler.Schedules.Single().Schedule.Next.Should().BeCloseTo(DateTime.Now.AddDays(1).AddHours(-1).ClearMinutes(), TimeSpan.FromSeconds(1)); 
        }

        [Test]
        public void TaskServer_SetDefault()
        {
            var server = TaskServer.Setup(_ => { }).SetDefault();
            TaskServer.Instance.Should().BeSameAs(server);
        }

        [Test]
        public void TaskServer_IsTaskScheduled()
        {
            var server = TaskServer.Setup(_ => { });
            server.Schedule<SimpleTask>(s => s.Every().Days().At(DateTime.Now.Hour - 1), "test");

            server.Scheduler.Schedules.First(s => s.Name == "test").Should().NotBeNull();
        }

        [Test]
        public void TaskServer_IsTaskScheduled_False()
        {
            TaskServer.Setup(_ => { }).IsTaskScheduled("test").Should().BeFalse();
        }

        public class AbortExecutionTask : IBackgroundTask
        {
            public void Execute(ExecutionContext context)
            {
                context.AbortExecution();
            }

            public void Dispose()
            {
            }
        }

        public class SimpleTask : IBackgroundTask
        {
            public void Execute(ExecutionContext context)
            {
            }

            public void Dispose()
            {
            }
        }
    }
}