using ScheduleIt.TimeUnits;

namespace ScheduleIt.Test.TimeUnits
{
    public class TimeUnitTests
    {
        [Test]
        public void NowUnit_AndEvery()
        {
            var schedule = new Schedule();
            var unit = new DateTimeUnit(schedule, DateTime.Now);
            var minutes = unit.AndEvery(10).Minutes();

            minutes.Calculate(DateTime.MinValue).Should().HaveMinute(10)
                .And.HaveDay(1)
                .And.HaveHour(0)
                .And.HaveSecond(0);
        }


        [Test]
        public void SNowUnit_AndEvery_TimeSpan()
        {
            var schedule = new Schedule();
            var unit = new DateTimeUnit(schedule, DateTime.Now);
            unit.AndEvery(TimeSpan.FromSeconds(30));

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddSeconds(30), TimeSpan.FromSeconds(1));
        }
    }
}
