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

        public abstract Task StartAsync();

        public abstract bool CollidesWith(BaseTask item);
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
