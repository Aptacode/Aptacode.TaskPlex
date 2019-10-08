using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskCoordinator.Tasks;

namespace TaskCoordinator
{
    public class TaskCoordinator
    {
        public TimeSpan SleepPeriod { get; set; }

        public List<BaseTask> pendingTasks;
        public List<BaseTask> runningTasks;
        private static readonly Object mutex = new Object();
        public bool IsRunning { get; set; }

        public TaskCoordinator()
        {
            pendingTasks = new List<BaseTask>();
            runningTasks = new List<BaseTask>();
            IsRunning = false;
            SleepPeriod = TimeSpan.FromMilliseconds(10);
        }

        public void Apply(BaseTask action)
        {
            pendingTasks.Add(action);
        }

        public void Start()
        {
            IsRunning = true;

            new TaskFactory().StartNew(() =>
            {
                run();
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void run()
        {
            while (IsRunning)
            {
                lock (mutex)
                {
                    //Start all transformations whose target does not collide with running transformations
                    List<BaseTask> startedTasks = new List<BaseTask>();
                    foreach (var item in pendingTasks)
                    {
                        if (!runningTasks.Exists(t => t.CollidesWith(item)))
                        {
                            runningTasks.Add(item);
                            startedTasks.Add(item);
                            //Remove the transformation from the running list when it finishes.
                            item.OnFinished += (s, e) =>
                            {
                                lock (mutex)
                                {
                                    runningTasks.Remove((BaseTask)s);
                                }
                            };
                            item.Start();
                        }
                    }

                    //Remove each started transformation from the pending transformation list
                    foreach (var item in startedTasks)
                    {
                        pendingTasks.Remove(item);
                    }
                }
                Thread.Sleep(SleepPeriod);
            }
        }
    }
}
