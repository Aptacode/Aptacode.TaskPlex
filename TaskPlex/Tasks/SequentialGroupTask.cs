using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class LinearGroupTaskEventArgs : BaseTaskEventArgs
    {
    }
    public class SequentialGroupTask : GroupTask
    {

        public SequentialGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
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

        protected override async Task InternalTask()
        {
            try
            {
                RaiseOnStarted(new LinearGroupTaskEventArgs());
                
                foreach (var task in Tasks)
                {
                    await task.StartAsync(_cancellationToken).ConfigureAwait(false);
                }

                RaiseOnFinished(new LinearGroupTaskEventArgs());
            }
            catch (TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }
    }
}
