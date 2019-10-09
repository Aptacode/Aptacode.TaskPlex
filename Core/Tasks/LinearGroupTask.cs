using System.Collections.Generic;
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
        List<BaseTask> Tasks { get; set; }

        public LinearGroupTask(IEnumerable<BaseTask> tasks) : base()
        {
            Tasks = new List<BaseTask>(tasks);
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
