using System;

namespace ScheduleIt.Diagnostics
{
    /// <summary>
    /// Delegate used in the default logger
    /// </summary>
    /// <param name="message"></param>
    /// <param name="level"></param>
    /// <param name="source"></param>
    /// <param name="function" >Name of the Method</param>
    public delegate void LogWriter(string message, LogLevel level, string source, string function);

    /// <summary>
    ///
    /// </summary>
    public static class LoggerFactory
    {
        private static ILog _logger;

        /// <summary>
        /// Gets the default <see cref="ILog"/>
        /// </summary>
        public static ILog Logger
        {
            get => _logger ?? new Logger();
            set => _logger = value;
        }

        /// <summary>
        /// Gets the default <see cref="LogWriter"/> used be the default <see cref="ILog"/>
        /// </summary>
        public static LogWriter DefaultWriter { get; set; } = (m, l, s, f) => Console.WriteLine($"{DateTime.Now:o} [{l.ToString()}] [{s}] [{f}] {m}");
    }
}
