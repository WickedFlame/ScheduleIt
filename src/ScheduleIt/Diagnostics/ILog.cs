
namespace ScheduleIt.Diagnostics
{
    /// <summary>
    /// Interface used for logging
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Write the Logmessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="source"></param>
        /// <param name="function"></param>
        void Write(string message, LogLevel level, string source, string function);
    }
}
