using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class ParallelGroupTaskEventArgs : BaseTaskEventArgs
    {
    }
    public class ParallelGroupTask : GroupTask
    {
        public ParallelGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }

        protected sealed override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
        {
            return tasks.Select(t => t.Duration).OrderByDescending(t => t.TotalMilliseconds).FirstOrDefault();
        }

        public override bool CollidesWith(BaseTask item)
        {
            return Tasks.Exists(t => t.CollidesWith(item));
        }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new ParallelGroupTaskEventArgs());
            await Task.WhenAll(Tasks.Select(task => task.StartAsync())); 
            RaiseOnFinished(new ParallelGroupTaskEventArgs());
        }

    }
}
