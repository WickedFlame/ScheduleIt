using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ScheduleIt
{
    /// <summary>
    /// The context containing objecs used for task esxecution
    /// </summary>
    public class ExecutionContext : IExecutionContext
    {
        private readonly IDictionary<string, object> _context;

        /// <summary>
        ///
        /// </summary>
        [DebuggerStepThrough]
        public ExecutionContext()
        {
            _context = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            CancellationToken = CancellationToken.None;
        }

        /// <summary>
        /// CancellationToken to stop the thread execution
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Set a object to the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IExecutionContext Set<T>(string key, T value)
        {
            _context[key] = value;
            return this;
        }

        /// <summary>
        /// Get a object that was previously set to the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return _context.TryGetValue(key, out var value) ? (T)value : default(T);
        }
    }
}
