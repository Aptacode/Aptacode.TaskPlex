using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskCoordinator.Tasks
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

        public override void Start()
        {
            RaiseOnStarted(new LinearGroupTaskEventArgs());

            new TaskFactory().StartNew(() =>
            {
                if (Tasks.Count > 0)
                {
                    chainTasks();
                    startChain();
                }else
                {
                    RaiseOnFinished(new LinearGroupTaskEventArgs());
                }
            });
        }

        private void chainTasks()
        {
            BaseTask lastTask = Tasks[0];

            for (int i = 1; i < Tasks.Count; i++)
            {
                BaseTask task = Tasks[i];
                lastTask.OnFinished += (s, e) =>
                {
                    task.Start();
                };

                lastTask = task;
            }

            lastTask.OnFinished += (s, e) =>
            {
                RaiseOnFinished(new LinearGroupTaskEventArgs());
            };
        }

        private void startChain()
        {
            Tasks[0].Start();
        }
    }
}
