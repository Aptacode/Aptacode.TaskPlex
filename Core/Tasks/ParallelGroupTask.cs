using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks
{
    public class ParallelGroupTaskEventArgs : BaseTaskEventArgs
    {
        public ParallelGroupTaskEventArgs()
        {

        }
    }
    public class ParallelGroupTask : GroupTask
    {
        List<BaseTask> Tasks { get; set; }

        public ParallelGroupTask(IEnumerable<BaseTask> tasks) : base(TimeSpan.Zero)
        {
            Tasks = new List<BaseTask>(tasks);
        }

        public override bool CollidesWith(BaseTask item)
        {
            return Tasks.Exists(t => t.CollidesWith(item));
        }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new ParallelGroupTaskEventArgs());
            var taskFactory = new TaskFactory();
            await Task.WhenAll(Tasks.Select(task => task.StartAsync()));
        }

    }
}
