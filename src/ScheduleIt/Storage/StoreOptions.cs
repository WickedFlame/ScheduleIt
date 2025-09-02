
namespace ScheduleIt.Storage
{
    /// <summary>
    /// Options to regulate the <see cref="ITaskStore"/>
    /// </summary>
    public class StoreOptions
    {
        /// <summary>
        /// Max entries contained in the store
        /// </summary>
        public int MaxEntries { get; set; } = 2000;

        /// <summary>
        /// Amount of entries that are removed when the store has reached the MaxEntries limit
        /// </summary>
        public int ShrinkCount { get; set; } = 500;
    }
}
