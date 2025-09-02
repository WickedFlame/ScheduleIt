using System;

namespace ScheduleIt.IoC
{
    /// <summary>
    /// A simple dependencyresolver
    /// </summary>
    public class BasicActivationContainer : IActivationContainer
    {
        /// <summary>
        /// Resolve a type and create a instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return Activator.CreateInstance<T>();
        }
    }
}
