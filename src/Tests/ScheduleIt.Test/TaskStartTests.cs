namespace ScheduleIt.Test
{
    public class TaskStartTests
    {
        [Test]
        public void TaskStart_StartNew()
        {
            var running = true;

            var starter = new TaskStart();
            starter.StartNew(() =>
            {
                while (running)
                {
                    // loop
                }
            });

            starter.Threads.Should().HaveCount(1);

            // cleanup
            running = false;
        }

        [Test]
        public void TaskStart_StartNew_Stop()
        {
            var running = true;

            var starter = new TaskStart();
            starter.StartNew(() =>
            {
                while (running)
                {
                    // loop
                }
            });

            running = false;
            Task.Delay(50).Wait();

            starter.Threads.Should().BeEmpty();
        }

        [Test]
        public void TaskStart_StartNew_Dispose()
        {
            var running = true;

            var starter = new TaskStart();
            starter.StartNew(() =>
            {
                while (running)
                {
                    starter.CancellationToken.ThrowIfCancellationRequested();
                    // loop
                }
            });

            starter.Dispose();

            Task.Delay(50).Wait();

            starter.Threads.Should().BeEmpty();
        }
    }
}
