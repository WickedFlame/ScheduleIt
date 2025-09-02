using System;
using System.Diagnostics;

namespace ScheduleIt
{
    /// <summary>
    /// A schedule that calculates a new time for execution
    /// </summary>
    [DebuggerDisplay("Next = {Next}")]
    public class Schedule
    {
        /// <summary>
        ///
        /// </summary>
        public Schedule()
        {
            Next = DateTime.MinValue;
            //Time = new ScheduleTime(0);
        }

        /// <summary>
        /// Gets the calculated <see cref="IScheduleTime"/>
        /// </summary>
        public IScheduleTime Time { get; private set; }

        /// <summary>
        /// Gets the next calculated DateTime for the Schedule
        /// </summary>
        public DateTime Next { get; set; }

        /// <summary>
        /// Calculate the next scheduled time based on the last schedule or execution
        /// </summary>
        /// <param name="last"></param>
        /// <returns></returns>
        public DateTime CalculateNext(DateTime last)
        {
            if (Time == null)
            {
                Next = DateTime.Now;
                return Next;
            }

            var next = Time.Calculate(last);

            Next = next != DateTime.MinValue && next < DateTime.Now ? DateTime.Now : next;

            var child = Time.GetChild();
            if (child != null)
            {
                Time = child;
            }

            return Next;
        }

        /// <summary>
        /// Set the time of the execution to the schedule
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(IScheduleTime time)
        {
            if (Time == null)
            {
                Time = time;
                return;
            }

            Time.SetChild(time);
        }
    }
}
