using System.Collections.Generic;
using System.Threading.Tasks;

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
        List<BaseTask> Tasks { get; set; }
        public int FinishedTaskCount { get; private set; }

        public ParallelGroupTask(IEnumerable<BaseTask> tasks) : base()
        {
            Tasks = new List<BaseTask>(tasks);
        }

        public override bool CollidesWith(BaseTask item)
        {
            return Tasks.Exists(t => t.CollidesWith(item));
        }

        public override void Start()
        {
            FinishedTaskCount = 0;
            RaiseOnStarted(new ParallelGroupTaskEventArgs());

            new TaskFactory().StartNew(() =>
            {
                connectTasks();
                startTasks();
            });
        }

        private void connectTasks()
        {
            foreach (var task in Tasks)
            {
                task.OnFinished += Task_OnFinished;
            }
        }

        private void Task_OnFinished(object sender, BaseTaskEventArgs eventArgs)
        {
            if (++FinishedTaskCount >= Tasks.Count)
                RaiseOnFinished(new ParallelGroupTaskEventArgs());
        }

        private void startTasks()
        {
            foreach (var task in Tasks)
            {
                task.Start();
            }
        }
    }
}
