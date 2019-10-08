using System;
using System.Collections.Generic;
using System.Text;

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
        public abstract event EventHandler<BaseTaskEventArgs> OnStarted;
        public abstract event EventHandler<BaseTaskEventArgs> OnFinished;

        public abstract void Start();
        public abstract bool CollidesWith(BaseTask item);
    }
}
