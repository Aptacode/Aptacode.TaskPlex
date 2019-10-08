using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCoordinator.Tasks
{
    public class GroupTaskEventArgs : BaseTaskEventArgs
    {
        public GroupTaskEventArgs()
        {

        }
    }
    public abstract class GroupTask : BaseTask
    {
        public override event EventHandler<BaseTaskEventArgs> OnStarted;
        public override event EventHandler<BaseTaskEventArgs> OnFinished;

        public TimeSpan Duration { get; set; }

        public GroupTask(TimeSpan duration)
        {
            Duration = duration;
        }

        public override bool CollidesWith(BaseTask item)
        {
            return false;
        }

        public override void Start()
        {
            OnStarted?.Invoke(this, new GroupTaskEventArgs());

            new TaskFactory().StartNew(() =>
            {
                Thread.Sleep(Duration);

            }).ContinueWith((e) =>
            {
                OnFinished?.Invoke(this, new GroupTaskEventArgs());
            });
        }
    }
}
