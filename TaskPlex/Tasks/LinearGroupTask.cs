using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class LinearGroupTaskEventArgs : BaseTaskEventArgs
    {
    }
    public class LinearGroupTask : GroupTask
    {

        public LinearGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }

        protected sealed override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (var task in tasks)
            {
                totalDuration = totalDuration.Add(task.Duration);
            }
            return totalDuration;
        }

        public override bool CollidesWith(BaseTask item)
        {
            return Tasks.Exists(t => t.CollidesWith(item));
        }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new LinearGroupTaskEventArgs());
            foreach (var task in Tasks)
            {
                await task.StartAsync().ConfigureAwait(false);
            } 
            RaiseOnFinished(new LinearGroupTaskEventArgs());
        }
    }
}
