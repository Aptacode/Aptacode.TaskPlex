using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks
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

        protected override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (var task in tasks)
            {
                totalDuration.Add(task.Duration);
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
