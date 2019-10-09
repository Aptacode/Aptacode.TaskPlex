using System;
using System.Threading.Tasks;

namespace TaskCoordinator.Tasks
{
    public class WaitTaskEventArgs : BaseTaskEventArgs
    {
        public WaitTaskEventArgs()
        {

        }
    }
    public class WaitTask : BaseTask
    {
        public TimeSpan Duration { get; set; }

        public WaitTask(TimeSpan duration)
        {
            Duration = duration;
        }

        public override bool CollidesWith(BaseTask item)
        {
            return false;
        }

        public override void Start()
        {
            RaiseOnStarted(new WaitTaskEventArgs());

            new TaskFactory().StartNew(() =>
            {
                Task.Delay(Duration);
            }).ContinueWith((e) =>
            {
                RaiseOnFinished(new WaitTaskEventArgs());
            });
        }
    }
}
