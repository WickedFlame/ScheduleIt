
namespace ScheduleIt.IoC
{
    /// <summary>
    /// Defines the methods used for creating objects
    /// </summary>
    public interface IActivationContainer
    {
        /// <summary>
        /// Resolve a type and create a instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;
    }
}
