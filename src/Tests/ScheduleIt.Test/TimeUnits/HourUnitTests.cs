using ScheduleIt.TimeUnits;

namespace ScheduleIt.Test.TimeUnits
{
    public class HourUnitTests
    {
        [Test]
        public void HourUnit_Calculate()
        {
            var unit = new HourUnit(10);
            unit.Calculate(DateTime.MinValue).Should().HaveHour(10)
                .And.HaveDay(1)
                .And.HaveSecond(0)
                .And.HaveMinute(0);
        }

        [Test]
        public void HourUnit_Calculate_At()
        {
            var unit = new HourUnit(10);
            unit.At(20);
            unit.Calculate(DateTime.MinValue).Should().HaveHour(10)
                .And.HaveDay(1)
                .And.HaveSecond(0)
                .And.HaveMinute(20);
        }
    }
}
