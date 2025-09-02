using ScheduleIt.Diagnostics;
using ScheduleIt.Scheduling;
using ScheduleIt.Storage;
using Monitor = ScheduleIt.Diagnostics.Monitor;

namespace ScheduleIt.Test.Diagnostics
{
    public class MonitorTests
    {
        private Mock<ITaskServer> _server;
        private List<TaskEntity> _tasks;
        private List<TaskSchedule> _schedules;

        [SetUp]
        public void Setup()
        {
            _tasks = [];
            var store = new Mock<ITaskStore>();
            store.Setup(x => x.GetEnumerator()).Returns(() => _tasks.GetEnumerator());
            
            _server = new Mock<ITaskServer>();
            _server.Setup(x => x.Store).Returns(() => store.Object);

            _schedules = [];
            _server.Setup(x => x.Scheduler.Schedules).Returns(() => _schedules);
        }
        
        [Test]
        public void Monitor_Running()
        {
            _tasks =
            [
                new TaskEntity { State = TaskState.Started }
            ];

            var monitor = new Monitor(_server.Object);
            monitor.Running.Should().HaveCount(1);
        }

        [Test]
        public void Monitor_Running_OnlyActive()
        {
            _tasks =
            [
                new TaskEntity { State = TaskState.Started },
                new TaskEntity { State = TaskState.Started },
                new TaskEntity { State = TaskState.Completed }
            ];
                
            var monitor = new Monitor(_server.Object);
            monitor.Running.Should().HaveCount(2);
        }

        [Test]
        public void Monitor_ScheduledTasks()
        {
            _schedules =
            [
                new TaskSchedule{ Name = "one", Schedule = new Schedule()},
                new TaskSchedule{ Name = "two", Schedule = new Schedule()}
            ];

            var monitor = new Monitor(_server.Object);
            monitor.ScheduledTasks.Should().HaveCount(2);
        }

        [Test]
        public void Monitor_ScheduledTasks_OnlyActive()
        {
            _schedules =
            [
                new TaskSchedule{ Name = "one", Schedule = new Schedule()}
            ];

            _tasks =
            [
                new TaskEntity { State = TaskState.Started },
                new TaskEntity { State = TaskState.Started },
                new TaskEntity { State = TaskState.Completed }
            ];
            
            var monitor = new Monitor(_server.Object);
            monitor.ScheduledTasks.Should().HaveCount(1);
        }

        [Test]
        public void Monitor_Extension()
        {
            using var server = new TaskServer();
            server.GetMonitor().Should().NotBeNull();
        }
    }
}
