
namespace ScheduleIt.Test
{
    public class TestTask : IBackgroundTask
    {
        private Action _task;

        public TestTask(Action task)
        {
            _task = task;
        }

        public void Execute(ExecutionContext context)
        {
            _task();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
