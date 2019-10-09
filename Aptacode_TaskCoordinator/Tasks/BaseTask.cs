using System;

namespace TaskCoordinator.Tasks
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

        public abstract void Start();
        public abstract bool CollidesWith(BaseTask item);
    }
}
