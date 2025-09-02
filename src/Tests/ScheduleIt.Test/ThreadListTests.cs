namespace ScheduleIt.Test
{
    public class ThreadListTests
    {
        [Test]
        public void ThreadList_Create()
        {
            new ThreadList().Should().BeEmpty();
        }

        [Test]
        public void ThreadList_Add()
        {
            var running = true;
            var list = new ThreadList();

            var task = Task.Factory.StartNew(() =>
            {
                while (running)
                {
                    // loop
                }
            });

            list.Add(task);

            list.Should().Contain(task);

            running = false;
        }

        [Test]
        public void ThreadList_RemoveOnEnd()
        {
            var running = true;
            var list = new ThreadList();

            var task = Task.Factory.StartNew(() =>
            {
                while (running)
                {
                    // loop
                }
            });

            list.Add(task);

            running = false;

            Task.Delay(50).Wait();

            list.Should().BeEmpty();
        }

        [Test]
        public void ThreadList_Dispose_Running()
        {
            var running = true;
            var list = new ThreadList();

            var task = Task.Factory.StartNew(() =>
            {
                while (running)
                {
                    // loop
                    list.CancellationToken.ThrowIfCancellationRequested();
                }
            }, list.CancellationToken);

            list.Add(task);

            var dispose = () => list.Dispose();
            dispose.Should().NotThrow();

            Task.Delay(100).Wait();

            list.Should().BeEmpty();
        }
    }
}
