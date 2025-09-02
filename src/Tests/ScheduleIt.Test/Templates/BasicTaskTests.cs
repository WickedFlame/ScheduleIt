using ScheduleIt.Templates;

namespace ScheduleIt.Test.Templates
{
    public class BasicTaskTests
    {
        [Test]
        public void BasicTask_Execute()
        {
            var ok = false;
            var task = new BasicTask(() => ok = true);
            task.Execute(new ExecutionContext());
            ok.Should().BeTrue();
        }

        [Test]
        public void BasicTask_Dispose()
        {
            var task = new BasicTask(() => { });
            var action = () => task.Dispose();
            action.Should().NotThrow();
        }
    }
}
