using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.EventArgs;

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

        /// <summary>
        ///     Wait task does not have any collisions
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool CollidesWith(IBaseTask item)
        {
            return false;
        }

        protected override async Task InternalTask()
        {
            try
            {
                RaiseOnStarted(new WaitTaskEventArgs());
                await Task.Delay(Duration, _cancellationToken.Token).ConfigureAwait(false);
                RaiseOnFinished(new WaitTaskEventArgs());
            }
            catch (TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }
    }
}