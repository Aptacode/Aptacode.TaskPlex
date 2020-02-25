using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class WaitTask : BaseTask
    {
        /// <summary>
        ///     Wait for a specified amount of time
        /// </summary>
        /// <param name="duration"></param>
        public WaitTask(TimeSpan duration) : base(duration)
        {
        }

        protected override async Task InternalTask()
        {
            if (Duration.TotalMilliseconds > 5)
            {
                await Task.Delay(Duration, CancellationTokenSource.Token).ConfigureAwait(false);
            }

            await WaitUntilResumed().ConfigureAwait(false);
        }
    }
}