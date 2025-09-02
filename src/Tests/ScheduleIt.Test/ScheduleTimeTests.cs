
namespace ScheduleIt.Test
{
    public class ScheduleTimeTests
    {
        [Test]
        public void ScheduleTime_Default()
        {
            var time = new ScheduleTime(10);
            time.Calculate(DateTime.MinValue).Should().HaveSecond(10)
                .And.HaveDay(1)
                .And.HaveHour(0)
                .And.HaveMinute(0);
        }

        [Test]
        public void ScheduleTime_Seconds()
        {
            var time = new ScheduleTime(10);
            time.Seconds();
            time.Calculate(DateTime.MinValue).Should().HaveSecond(10)
                .And.HaveDay(1)
                .And.HaveHour(0)
                .And.HaveMinute(0);
        }

        [Test]
        public void ScheduleTime_Minutes()
        {
            var time = new ScheduleTime(10);
            time.Minutes();

            time.Calculate(DateTime.MinValue).Should().HaveMinute(10)
                .And.HaveDay(1)
                .And.HaveHour(0)
                .And.HaveSecond(0);
        }

        [Test]
        public void ScheduleTime_Hours()
        {
            var time = new ScheduleTime(10);
            time.Hours();

            time.Calculate(DateTime.MinValue).Should().HaveHour(10)
                .And.HaveDay(1)
                .And.HaveSecond(0)
                .And.HaveMinute(0);
        }

        [Test]
        public void ScheduleTime_Days()
        {
            var time = new ScheduleTime(10);
            time.Days();

            time.Calculate(DateTime.MinValue).Should().HaveDay(11)
                .And.HaveHour(0)
                .And.HaveMinute(0)
                .And.HaveSecond(0);
        }
    }
}
