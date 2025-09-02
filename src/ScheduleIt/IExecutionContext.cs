
namespace ScheduleIt
{
    /// <summary>
    /// Defines the context containing objecs used for task esxecution
    /// </summary>
    public interface IExecutionContext
    {
        /// <summary>
        /// Set a object to the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IExecutionContext Set<T>(string key, T value);

        /// <summary>
        /// Get a object that was previously set to the context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
    }
}
