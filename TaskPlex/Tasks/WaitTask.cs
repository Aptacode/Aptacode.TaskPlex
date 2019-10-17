using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class WaitTaskEventArgs : BaseTaskEventArgs
    {

    }
    public class WaitTask : BaseTask
    {
        public WaitTask(TimeSpan duration) : base(duration)
        {

        }

        public override bool CollidesWith(BaseTask item)
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
            catch(TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }
    }
}
