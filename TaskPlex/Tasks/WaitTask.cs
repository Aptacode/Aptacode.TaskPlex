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
                await Task.Delay(Duration, CancellationToken.Token).ConfigureAwait(false);
            }

            await WaitUntilResumed().ConfigureAwait(false);
        }

        public override bool Equals(object obj)
        {
            return obj is WaitTask task && task.GetHashCode() == GetHashCode();
        }
    }
}