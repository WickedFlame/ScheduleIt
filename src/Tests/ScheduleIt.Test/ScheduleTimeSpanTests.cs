
namespace ScheduleIt.Test
{
    public class ScheduleTimeSpanTests
    {
        [Test]
        public void ScheduleTimeSpan_Calculate()
        {
            var time = new ScheduleTimeSpan(TimeSpan.FromSeconds(30));
            time.Calculate(DateTime.MinValue).Should().HaveSecond(30);
        }
    }
}
