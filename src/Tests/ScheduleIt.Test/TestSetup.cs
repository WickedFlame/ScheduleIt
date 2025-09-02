using ScheduleIt.Diagnostics;

namespace ScheduleIt.Test
{
    [SetUpFixture]
    public class TestSetup
    {
        [OneTimeSetUp]
        public void Setup()
        {
            LoggerFactory.DefaultWriter = (m, l, s, f) => { };
        }
    }
}
