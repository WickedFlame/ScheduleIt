
namespace ScheduleIt.TestApp
{
    public class TestTask : IBackgroundTask
    {
        private readonly string _name;

        public TestTask()
        {
            var rnd = new Random();
            _name = $"{nameof(TestTask)} {rnd.Next(10)}";
        }

        public TestTask(string name)
        {
            var rnd = new Random();
            _name = $"{name} {rnd.Next(10)}";
        }

        public void Execute(ExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now:o} [{_name}]");
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}
