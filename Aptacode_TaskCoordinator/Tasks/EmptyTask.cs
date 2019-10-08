using System;
using System.Threading;
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
        public override event EventHandler<BaseTaskEventArgs> OnStarted;
        public override event EventHandler<BaseTaskEventArgs> OnFinished;

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
            OnStarted?.Invoke(this, new WaitTaskEventArgs());

            new TaskFactory().StartNew(() =>
            {
                Thread.Sleep(Duration);

            }).ContinueWith((e) =>
            {
                OnFinished?.Invoke(this, new WaitTaskEventArgs());
            });
        }
    }
}
