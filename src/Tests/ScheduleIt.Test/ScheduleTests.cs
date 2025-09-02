using ScheduleIt.TimeUnits;

namespace ScheduleIt.Test
{
    public class ScheduleTests
    {
        [Test]
        public void Schedule_Default()
        {
            var schedule = new Schedule();
            schedule.Next.Should().BeSameDateAs(DateTime.MinValue);
        }

        [Test]
        public void Schedule_Default_CalculateNext()
        {
            var schedule = new Schedule();
            schedule.CalculateNext(DateTime.Now.AddMinutes(-1));

            schedule.Next.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_DefaultUnit()
        {
            var schedule = new Schedule();
            schedule.Every(30);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddSeconds(30), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_SecondUnit()
        {
            var schedule = new Schedule();
            schedule.Every(30).Seconds();

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddSeconds(30), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_MinuteUnit()
        {
            var schedule = new Schedule();
            schedule.Every(30).Minutes();

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddMinutes(30), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_HourUnit()
        {
            var schedule = new Schedule();
            schedule.Every(2).Hours();

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddHours(2), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_HourUnit_At()
        {
            var schedule = new Schedule();
            schedule.Every(2).Hours().At(20);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddHours(2).ClearMinutes().AddMinutes(20), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_DayUnit()
        {
            var schedule = new Schedule();
            schedule.Every(2).Days();

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddDays(2), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_DayUnit_AtHour()
        {
            var schedule = new Schedule();
            schedule.Every(2).Days().At(3);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddDays(2).Date.ClearMinutes().AddHours(3), TimeSpan.FromSeconds(1));
        }



        [Test]
        public void Schedule_Every_DayUnit_AtHour_Past()
        {
            var schedule = new Schedule();

            var hour = DateTime.Now.Hour - 1;

            schedule.Every().Days().At(hour);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddDays(1).Date.ClearMinutes().AddHours(hour).ClearMinutes(), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_DayUnit_AtHour_Today()
        {
            var schedule = new Schedule();

            var hour = DateTime.Now.Hour + 1;

            schedule.Every().Days().At(hour);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Today.Date.ClearMinutes().AddHours(hour).ClearMinutes(), TimeSpan.FromSeconds(1));
        }





        [Test]
        public void Schedule_Every_DayUnit_AtHourMinute()
        {
            var schedule = new Schedule();
            schedule.Every(2).Days().At(3, 20);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddDays(2).Date.ClearMinutes().AddHours(3).AddMinutes(20), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_DayUnit_AtHourMinute_Past()
        {
            var schedule = new Schedule();

            var hour = DateTime.Now.Hour - 1;

            schedule.Every().Days().At(hour , 10);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddDays(1).Date.ClearMinutes().AddHours(hour).AddMinutes(10), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_DayUnit_AtHourMinute_Today()
        {
            var schedule = new Schedule();

            var hour = DateTime.Now.Hour + 1;

            schedule.Every().Days().At(hour, 10);

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Today.Date.ClearMinutes().AddHours(hour).AddMinutes(10), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_TimeSpan()
        {
            var schedule = new Schedule();
            schedule.Every(TimeSpan.FromSeconds(30));

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddSeconds(30), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_At()
        {
            var schedule = new Schedule();
            schedule.At(DateTime.Now.AddMinutes(1));

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_At_Past()
        {
            var schedule = new Schedule();
            schedule.At(DateTime.Now.AddMinutes(-1));

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void Schedule_In()
        {
            var schedule = new Schedule();
            schedule.In(TimeSpan.FromMinutes(1));

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_In_Past()
        {
            var schedule = new Schedule();
            schedule.In(TimeSpan.FromMinutes(-1));

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void Schedule_Next_In_Default()
        {
            var schedule = new Schedule();
            schedule.In(TimeSpan.FromMinutes(1));

            schedule.CalculateNext(DateTime.Now);

            schedule.Next.Should().BeCloseTo(DateTime.Now.AddMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Past()
        {
            var schedule = new Schedule();
            schedule.Every(TimeSpan.FromSeconds(30));

            // if the scheduled time is in the past
            // the next should be now
            schedule.CalculateNext(DateTime.Now.AddMinutes(-1));

            schedule.Next.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Next_Default()
        {
            var schedule = new Schedule();
            schedule.Every(TimeSpan.FromSeconds(30));

            schedule.Next.Should().Be(DateTime.MinValue);
        }

        [Test]
        public void Schedule_Next_IsSet()
        {
            var schedule = new Schedule();
            schedule.Every(TimeSpan.FromSeconds(30));

            // if the scheduled time is in the past
            // the next should be now
            schedule.CalculateNext(DateTime.Now).Should().Be(schedule.Next);
        }

        [Test]
        public void Schedule_TimeToNext()
        {
            var time = TimeSpan.FromSeconds(30);
            var schedule = new Schedule();
            schedule.Every(time);
            schedule.CalculateNext(DateTime.Now);

            // if the scheduled time is in the past
            // the next should be now
            schedule.TimeToNext().Should().BeCloseTo(time, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_TimeToNext_NotSet()
        {
            var time = TimeSpan.FromSeconds(30);
            var schedule = new Schedule();
            schedule.Every(time);

            // next is not calculated so Next is DateTime.MinValue
            // Results in TimeSpan.Zero
            schedule.TimeToNext().Should().Be(TimeSpan.Zero);
        }

        [Test]
        public void Schedule_Now()
        {
            var schedule = new Schedule();
            schedule.Now();

            schedule.Next.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Now_TimeToNext()
        {
            var schedule = new Schedule();
            schedule.Now();

            schedule.TimeToNext().Should().BeLessThan(TimeSpan.Zero);
        }

        [Test]
        public void Schedule_Now_NowUnit()
        {
            var schedule = new Schedule();
            schedule.Now().Should().BeOfType<DateTimeUnit>();
        }

        [Test]
        public void Schedule_Now_AndEvery()
        {
            var schedule = new Schedule();
            schedule.Now().AndEvery(1).Minutes();

            schedule.TimeToNext().Should().BeLessThan(TimeSpan.Zero);
        }

        [Test]
        public void Schedule_Now_AndEvery_CalculateNext()
        {
            var schedule = new Schedule();
            schedule.Now().AndEvery(1).Minutes();
            schedule.CalculateNext(DateTime.Now);

            // now
            schedule.TimeToNext().Should().Be(TimeSpan.Zero);

            // every
            schedule.CalculateNext(DateTime.Now);

            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_In_AndEvery_CalculateNext()
        {
            var schedule = new Schedule();
            schedule.In(TimeSpan.FromMilliseconds(100)).AndEvery(1).Minutes();
            schedule.CalculateNext(DateTime.Now);

            // in
            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));

            // every
            schedule.CalculateNext(DateTime.Now);

            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_In_AndEvery_DT_CalculateNext()
        {
            var schedule = new Schedule();
            schedule.In(TimeSpan.FromMilliseconds(100)).AndEvery(TimeSpan.FromMinutes(1));
            schedule.CalculateNext(DateTime.Now);

            // in
            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));

            // every
            schedule.CalculateNext(DateTime.Now);

            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_Every_CalculateNext()
        {
            var schedule = new Schedule();
            schedule.Every(1).Minutes();
            schedule.CalculateNext(DateTime.Now);

            // every
            schedule.CalculateNext(DateTime.Now);

            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_At_AndEvery_CalculateNext()
        {
            var schedule = new Schedule();
            schedule.At(DateTime.Now.AddMilliseconds(100)).AndEvery(1).Minutes();
            schedule.CalculateNext(DateTime.Now);

            // in
            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));

            // every
            schedule.CalculateNext(DateTime.Now);

            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Schedule_At_AndEvery_DT_CalculateNext()
        {
            var schedule = new Schedule();
            schedule.At(DateTime.Now.AddMilliseconds(100)).AndEvery(TimeSpan.FromMinutes(1));
            schedule.CalculateNext(DateTime.Now);

            // in
            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));

            // every
            schedule.CalculateNext(DateTime.Now);

            schedule.TimeToNext().Should().BeCloseTo(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1));
        }
    }
}