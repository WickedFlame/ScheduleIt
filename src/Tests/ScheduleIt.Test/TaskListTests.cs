namespace ScheduleIt.Test
{
    public class TaskListTests
    {
        [Test]
        public void TaskList_Create()
        {
            new TaskList().Should().BeEmpty();
        }

        [Test]
        public void TaskList_Add()
        {
            var task = new Mock<IBackgroundTask>();
            var list = new TaskList();
            list.Add(task.Object);

            list.Should().HaveCount(1);
        }

        [Test]
        public void TaskList_Remove()
        {
            var task = new Mock<IBackgroundTask>();
            var list = new TaskList { task.Object };

            list.Remove(task.Object);

            list.Should().BeEmpty();
        }
    }
}
