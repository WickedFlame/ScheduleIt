using ScheduleIt.TimeUnits;

namespace ScheduleIt.Test.TimeUnits
{
    public class DayUnitTests
    {
        [Test]
        public void DayUnit_Calculate()
        {
            var unit = new DayUnit(10);
            unit.Calculate(DateTime.MinValue).Should().HaveDay(11)
                .And.HaveHour(0)
                .And.HaveMinute(0)
                .And.HaveSecond(0);
        }

        [Test]
        public void DayUnit_Calculate_At_Hour()
        {
            var unit = new DayUnit(10);
            unit.At(2);

            unit.Calculate(DateTime.MinValue).Should().HaveDay(1)
                .And.HaveHour(2)
                .And.HaveMinute(0)
                .And.HaveSecond(0);
        }

        [Test]
        public void DayUnit_Calculate_At_Hour_Next()
        {
            var unit = new DayUnit(10);
            unit.At(2);

            var next = unit.Calculate(DateTime.MinValue);
            unit.Calculate(next).Should().HaveDay(11)
                .And.HaveHour(2)
                .And.HaveMinute(0)
                .And.HaveSecond(0);
        }

        [Test]
        public void DayUnit_Calculate_At_HourMinute()
        {
            var unit = new DayUnit(10);
            unit.At(2, 20);

            //
            // Es wird darauf geschaut dass die Zeit an dem Tag noch nicht stattgefunden hat
            // Sonst wird der interval dazugerechnet
            unit.Calculate(DateTime.MinValue).Should().HaveDay(1)
                .And.HaveHour(2)
                .And.HaveMinute(20)
                .And.HaveSecond(0);
        }

        [Test]
        public void DayUnit_Calculate_At_HourMinute_Next()
        {
            var unit = new DayUnit(10);
            unit.At(2, 20);

            //
            // Es wird darauf geschaut dass die Zeit an dem Tag noch nicht stattgefunden hat
            // Sonst wird der interval dazugerechnet
            var next = unit.Calculate(DateTime.MinValue);
            unit.Calculate(next).Should().HaveDay(11)
                .And.HaveHour(2)
                .And.HaveMinute(20)
                .And.HaveSecond(0);
        }

        [Test]
        public void DayUnit_Calculate_At_HourMinute_Past()
        {
            var unit = new DayUnit(10);
            unit.At(2, 20);

            //
            // last execution: 1.1.0001 5:00:00
            // Execution jeden Tag um 2:20:00 somit früher als last execution
            // Interval von DayUnit wird dazugerechnet
            unit.Calculate(DateTime.MinValue.AddHours(5)).Should().HaveDay(11)
                .And.HaveHour(2)
                .And.HaveMinute(20)
                .And.HaveSecond(0);
        }
    }
}
