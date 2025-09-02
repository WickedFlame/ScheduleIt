using ScheduleIt.Diagnostics;
using ScheduleIt.IoC;

namespace ScheduleIt.Test
{
    public class TaskServerBuilderTests
    {
        [Test]
        public void TaskServerBuilder_UseActivationContainer()
        {
            var container = new Mock<IActivationContainer>();

            var builder = new TaskServerBuilder();
            builder.UseActivationContainer(container.Object);

            builder.Build().Resolver.Should().Be(container.Object);
        }

        [Test]
        public void TaskServerBuilder_UseLogger_LogWriter()
        {
            var defaultWriter = LoggerFactory.DefaultWriter;

            var logger = new Mock<LogWriter>();

            var builder = new TaskServerBuilder();
            builder.UseLogger(logger.Object);

            builder.Build();

            LoggerFactory.DefaultWriter.Should().Be(logger.Object);

            // reset
            LoggerFactory.DefaultWriter = defaultWriter;
        }

        [Test]
        public void TaskServerBuilder_UseLogger_ILog()
        {
            var logger = new Mock<ILog>();

            var builder = new TaskServerBuilder();
            builder.UseLogger(logger.Object);

            builder.Build();

            LoggerFactory.Logger.Should().Be(logger.Object);

            // reset
            LoggerFactory.Logger = null;
        }

        [Test]
        public void TaskServerBuilder_SetServer()
        {
            var server = new TaskServer();

            var builder = new TaskServerBuilder();
            builder.SetServer(server);

            builder.Build().Should().BeSameAs(server);
        }

        [Test]
        public void TaskServerBuilder_UseScheduler()
        {
            var scheduler = new Scheduler();

            var builder = new TaskServerBuilder();
            builder.UseScheduler(scheduler);

            builder.Build().Scheduler.Should().BeSameAs(scheduler);
        }
    }
}
