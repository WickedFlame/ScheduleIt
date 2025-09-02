using ScheduleIt.Storage;
using ScheduleIt.Templates;
using System.Diagnostics;

namespace ScheduleIt.Test
{
    [SingleThreaded]
    [Ignore("Zeitgesteuerte Integrationstests schlagen auf Buildserver oft fehl...")]
    public class TaskServerTestsIntegration
    {
        [Test]
        public void TaskServer_StartNew_Tasks_Remove()
        {
            var running = true;
            var task = new TestTask(() =>
            {
                while (running)
                {
                    Task.Delay(50).Wait();
                }
            });

            var server = new TaskServer();
            server.StartNew(task);

            server.Tasks.Should().HaveCount(1).And.Contain(task);

            running = false;

            Task.Delay(150).Wait();

            server.Tasks.Should().BeEmpty();
        }

        [Test]
        public void TaskServer_Schedule_Dispose()
        {
            var task = new TestTask(() =>
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:o}] [TaskServerTests] [Debug] run");
            });

            var server = new TaskServer();
            server.Schedule(task, s => s.Every(TimeSpan.FromMilliseconds(1)));

            Task.Delay(50).Wait();

            server.Dispose();

            Task.Delay(150).Wait();

            server.Tasks.Should().BeEmpty();
        }

        [Test]
        public void TaskServer_Schedule_SchedulerInTasks()
        {
            var task = new TestTask(() =>
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:o}] [TaskServerTests] [Debug] run");
            });

            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(task, s => s.Every(TimeSpan.FromMilliseconds(1)));

            Task.Delay(50).Wait();

            server.Tasks.Should().Contain(t => t is IScheduler);
            server.Dispose();
        }

        [Test]
        public void TaskServer_Schedule_Executed()
        {
            var executed = false;

            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(() => new TestTask(() => executed = true), s => s.Now(), "Test");

            Task.Delay(50).Wait();

            executed.Should().BeTrue();
        }

        [Test]
        public void TaskServer_Schedule_Tasks()
        {
            var task = new TestTask(() =>
            {
                Debug.WriteLine($"Execution {DateTime.Now:o}");
                Task.Delay(300).Wait();
            });

            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(() => task, s => s.Now().AndEvery(TimeSpan.FromMilliseconds(100)), "test");

            Task.Delay(100).Wait();

            server.Tasks.Should().HaveCountGreaterThan(2);
            server.Dispose();
        }

        [Test]
        public void TaskServer_Schedule_Tasks_Every()
        {
            var task = new TestTask(() =>
            {
                Debug.WriteLine($"Execution {DateTime.Now:o}");
                Task.Delay(300).Wait();
            });

            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            Debug.WriteLine($"Start {DateTime.Now:o}");
            server.Schedule(() => task, s => s.Every(TimeSpan.FromMilliseconds(100)), "test");

            Task.Delay(150).Wait();

            server.Tasks.Should().HaveCountGreaterThanOrEqualTo(1).And.HaveCountLessThanOrEqualTo(2);

            server.Dispose();
        }

        [Test]
        public void TaskServer_Schedule_Tasks_In_AndEvery()
        {
            var count = 0;
            var task = new TestTask(() =>
            {
                count++;
            });

            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(() => task, s => s.In(TimeSpan.FromMilliseconds(20)).AndEvery(TimeSpan.FromMilliseconds(20)), "test");

            Task.Delay(100).Wait();

            server.Dispose();

            server.Store.Where(s => s.State == TaskState.Completed).Should().HaveCountGreaterThanOrEqualTo(1).And.HaveCount(count);            
        }

        [Test]
        public void TaskServer_Setup_StoreOptions_Entries()
        {
            var options = new Storage.StoreOptions();
            using var server = TaskServer.Setup(b => b.UseStoreOptions(options));
            server.Schedule(new Mock<IBackgroundTask>().Object, s => s.Now());

            Task.Delay(50).Wait();

            // scheduler is not added to the store
            server.Store.Should().HaveCount(1);
        }

        [Test]
        public void TaskServer_Setup_StoreOptions_State()
        {
            var options = new Storage.StoreOptions();
            using (var server = TaskServer.Setup(b => b.UseStoreOptions(options)))
            {
                server.Schedule(new Mock<IBackgroundTask>().Object, s => s.Now());

                Task.Delay(50).Wait();

                server.Store.First(e => e.TaskType == "IBackgroundTaskProxy").State.Should().Be(TaskState.Completed);
            }
        }

        [Test]
        public void TaskServer_Setup_StoreOptions_Duration()
        {
            var options = new Storage.StoreOptions();
            using (var server = TaskServer.Setup(b => b.UseStoreOptions(options)))
            {
                server.Schedule(new Mock<IBackgroundTask>().Object, s => s.Now());

                Task.Delay(50).Wait();

                server.Store.First(e => e.TaskType == "IBackgroundTaskProxy").Duration.Should().BeGreaterThan(TimeSpan.Zero);
            }
        }

        [Test]
        public void TaskServer_Setup_StoreOptions_StartTime()
        {
            var options = new Storage.StoreOptions();
            using (var server = TaskServer.Setup(b => b.UseStoreOptions(options)))
            {
                server.Schedule(new Mock<IBackgroundTask>().Object, s => s.Now());

                Task.Delay(50).Wait();

                server.Store.First(e => e.TaskType == "IBackgroundTaskProxy").StartTime.Should().BeBefore(DateTime.Now);
            }
        }

        [Test]
        public void TaskServer_StartNew_State()
        {
            var task = new TestTask(() => { });

            var server = new TaskServer();
            server.StartNew(task);

            Task.Delay(50).Wait();

            server.Store.Single().State.Should().Be(TaskState.Completed);
        }

        [Test]
        public void TaskServer_StartNew_State_FailingTask()
        {
            var task = new TestTask(() => throw new Exception());

            var server = new TaskServer();
            server.StartNew(task);

            Task.Delay(100).Wait();

            server.Store.Single().State.Should().Be(TaskState.Error);
        }




        [Test]
        public void TaskServer_Schedule_Action()
        {
            var executed = false;

            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(() => executed = true, s => s.Now());

            Task.Delay(50).Wait();

            executed.Should().BeTrue();
        }

        [Test]
        public void TaskServer_Schedule_Action_Name()
        {
            var executed = false;

            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(() => executed = true, s => s.Now(), "Test");

            Task.Delay(50).Wait();

            executed.Should().BeTrue();
        }

        [Test]
        public void TaskServer_Schedule_Action_GeneratedTask()
        {
            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(() => { }, s => s.Now());

            Task.Delay(50).Wait();

            server.Store.Single().TaskType.Should().Be(nameof(BasicTask));
        }

        [Test]
        public void TaskServer_Schedule_Action_Name_NeneratedTask()
        {
            var server = new TaskServer();

            server.Tasks.Should().BeEmpty();

            server.Schedule(() => { }, s => s.Now(), "Test");

            Task.Delay(50).Wait();

            server.Store.Single().TaskType.Should().Be(nameof(BasicTask));
        }

        [Test]
        public void TaskServer_AbortExecution()
        {
            var server = new TaskServer();

            server.StartNew(new AbortExecutionTask());

            Task.Delay(10).Wait();

            server.Store.Single().State.Should().Be(TaskState.Aborted);
        }

        [Test]
        public void TaskServer_Schedule_AbortExecution()
        {
            var server = new TaskServer();

            server.Schedule(new AbortExecutionTask(), s => s.Every(TimeSpan.FromMilliseconds(5)));

            Task.Delay(50).Wait();
            server.Dispose();

            server.Store.Should().HaveCountGreaterThan(0);
            server.Store.All(s => s.State == TaskState.Aborted).Should().BeTrue();
        }

        [Test]
        public void TaskServer_Remove()
        {
            var task = new TestTask(() => Task.Delay(2).Wait());

            using var server = new TaskServer();
            server.Schedule(() => task, s => s.Now().AndEvery(TimeSpan.FromMilliseconds(5)), "test");

            Task.Delay(50).Wait();

            server.Remove("test");

            Task.Delay(20).Wait();

            server.Store.Should().NotBeEmpty();
            server.Tasks.Should().BeEmpty();
            server.Scheduler.Schedules.Should().BeEmpty();
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
    }
}