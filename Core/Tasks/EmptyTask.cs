using System;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks
{
    public class WaitTaskEventArgs : BaseTaskEventArgs
    {
        public WaitTaskEventArgs()
        {

        }
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

        public override async Task StartAsync()
        {
            RaiseOnStarted(new WaitTaskEventArgs());

            await Task.Delay(Duration);

            RaiseOnFinished(new WaitTaskEventArgs());
        }
    }
}
