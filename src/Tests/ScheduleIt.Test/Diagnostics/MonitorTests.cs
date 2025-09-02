using Monitor = ScheduleIt.Diagnostics.Monitor;

namespace ScheduleIt.Test.Diagnostics
{
    public class MonitorTests
    {
        [Test]
        public void Monitor_Running()
        {
            using var server = new TaskServer();
            server.Schedule(new TestTask(() => { Task.Delay(100).Wait(); }), s => s.Now());

            Task.Delay(50).Wait();

            var monitor = new Monitor(server);
            monitor.Running.Should().HaveCount(1);
        }

        [Test]
        public async Task Monitor_Running_OnlyActive()
        {
            using var server = new TaskServer();
            server.Schedule(new TestTask(() => { Task.Delay(100).Wait(); }), s => s.Now().AndEvery(2).Seconds());
            server.Schedule(new TestTask(() => { Task.Delay(100).Wait(); }), s => s.Now());
            server.Schedule(new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(1)));

            await Task.Delay(50);

            var monitor = new Monitor(server);
            monitor.Running.Should().HaveCount(2);
        }

        [Test]
        public void Monitor_ScheduledTasks()
        {
            using var server = new TaskServer();
            server.Schedule(new TestTask(() => { }), s => s.Now().AndEvery(2).Seconds());
            server.Schedule(new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(1)));

            var monitor = new Monitor(server);
            monitor.ScheduledTasks.Should().HaveCount(2);
        }

        [Test]
        public void Monitor_ScheduledTasks_OnlyActive()
        {
            using var server = new TaskServer();
            server.Schedule(new TestTask(() => { }), s => s.Now());
            server.Schedule(new TestTask(() => { }), s => s.In(TimeSpan.FromMinutes(1)));

            Task.Delay(200).Wait();

            var monitor = new Monitor(server);
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
