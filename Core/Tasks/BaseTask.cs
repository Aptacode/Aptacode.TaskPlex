using System;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks
{
    public abstract class BaseTaskEventArgs : EventArgs
    {
        public BaseTaskEventArgs()
        {

        }
    }
    public abstract class BaseTask
    {
        public event EventHandler<BaseTaskEventArgs> OnStarted;
        public event EventHandler<BaseTaskEventArgs> OnFinished;

        public TimeSpan Duration { get; set; }

        public BaseTask(TimeSpan duration)
        {
            Duration = duration;
        }

        public BaseTask()
        {
            Duration = TimeSpan.Zero;
        }

        public abstract bool CollidesWith(BaseTask item);
        public abstract Task StartAsync();

        protected void RaiseOnStarted(BaseTaskEventArgs args)
        {
            OnStarted?.Invoke(this, args);
        }
        protected void RaiseOnFinished(BaseTaskEventArgs args)
        {
            OnFinished?.Invoke(this, args);
        }
    }
}
