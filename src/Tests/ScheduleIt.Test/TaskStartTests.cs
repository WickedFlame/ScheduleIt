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
            var starter = new TaskStart();
            starter.StartNew(() =>
            {
                while (true)
                {
                    starter.CancellationToken.ThrowIfCancellationRequested();
                    // loop
                }
            });

            starter.Dispose();

            // Wait for thread to exit by polling Threads property
            SpinWait.SpinUntil(() => !starter.Threads.Any(), 1000);

            starter.Threads.Should().BeEmpty();
        }
    }
}
