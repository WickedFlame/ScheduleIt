using ScheduleIt.TimeUnits;

namespace ScheduleIt.Test.TimeUnits
{
    public class SecondUnitTests
    {
        [Test]
        public void SecondUnit_Calculate()
        {
            var unit = new SecondUnit(10);
            unit.Calculate(DateTime.MinValue).Should().HaveSecond(10)
                .And.HaveDay(1)
                .And.HaveHour(0)
                .And.HaveMinute(0);
        }
    }
}
