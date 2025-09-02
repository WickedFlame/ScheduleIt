using ScheduleIt.Templates;

namespace ScheduleIt.Test
{
    public class TaskExecutionTests
    {
        [Test]
        public void TaskExecution_ExecuteSubtask()
        {
            var context = new ExecutionContext();
            context.Set(nameof(TaskServer), new TaskServer());
            var task = new ContextTask(x =>
            {
                x.ExecuteSubtask<SubTask>();
            });
            task.Execute(context);

            context.Get<bool>("subtask").Should().BeTrue();
        }

        public class SubTask : IBackgroundTask
        {
            public void Dispose()
            {
            }

            public void Execute(ExecutionContext context)
            {
                context.Set("subtask", true);
            }
        }
    }
}
