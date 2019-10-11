using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks
{
    public class LinearGroupTaskEventArgs : BaseTaskEventArgs
    {
        public LinearGroupTaskEventArgs()
        {

        }
    }
    public class LinearGroupTask : GroupTask
    {

        public LinearGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
        {

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
                await task.StartAsync();
            }

            RaiseOnFinished(new LinearGroupTaskEventArgs());
        }
    }
}
