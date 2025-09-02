
namespace ScheduleIt.Diagnostics
{
    internal class Logger : ILog
    {
        /// <summary>
        /// Write the Logmessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="source"></param>
        /// <param name="function">Name of the Method</param>
        public void Write(string message, LogLevel level, string source, string function)
        {
             LoggerFactory.DefaultWriter(message, level, source, function);
        }
    }
}
