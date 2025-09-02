
namespace ScheduleIt
{
    /// <summary>
    /// Base class for schedules
    /// </summary>
    public abstract class ScheduleBase
    {
        private IScheduleTime _child;

        /// <summary>
        /// Get the next scheduletime
        /// </summary>
        /// <returns></returns>
        public IScheduleTime GetChild()
        {
            return _child;
        }

        /// <summary>
        /// Add a further scheduletime that is executed after the current
        /// </summary>
        /// <param name="time"></param>
        public void SetChild(IScheduleTime time)
        {
            if (_child != null)
            {
                _child.SetChild(time);
                return;
            }

            _child = time;
        }
    }
}
