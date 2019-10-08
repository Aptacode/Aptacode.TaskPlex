using System;
using System.Collections.Generic;

namespace TaskCoordinator.Tasks
{
    public class ParallelGroupTaskEventArgs : BaseTaskEventArgs
    {
        public ParallelGroupTaskEventArgs()
        {

        }
    }
    public class ParallelGroupTask : GroupTask
    {
        public override event EventHandler<BaseTaskEventArgs> OnStarted;
        public override event EventHandler<BaseTaskEventArgs> OnFinished;
        List<BaseTask> Tasks { get; set; }

        public ParallelGroupTask(IEnumerable<BaseTask> tasks) : base()
        {
            Tasks = new List<BaseTask>(tasks);
        }

        public override bool CollidesWith(BaseTask item)
        {
            return Tasks.Exists(t => t.CollidesWith(item));
        }

        public int FinishedTaskCount { get; set; }
        public override void Start()
        {
            FinishedTaskCount = 0;

            OnStarted?.Invoke(this, new ParallelGroupTaskEventArgs());
            foreach(var task in Tasks)
            {
                task.OnFinished += (s, e) =>
                {
                    if (++FinishedTaskCount >= Tasks.Count)
                        OnFinished?.Invoke(this, new ParallelGroupTaskEventArgs());
                };

                task.Start();
            }

        }
    }
}
