using ScheduleIt.TimeUnits;

namespace ScheduleIt.Test.TimeUnits
{
    public class MinuteUnitTests
    {
        [Test]
        public void MinuteUnit_Calculate()
        {
            var unit = new MinuteUnit(10);
            unit.Calculate(DateTime.MinValue).Should().HaveMinute(10)
                .And.HaveDay(1)
                .And.HaveHour(0)
                .And.HaveSecond(0);
        }
    }
}
