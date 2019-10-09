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

        private readonly List<BaseTask> pendingTasks;
        private readonly List<BaseTask> runningTasks;
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
                    List<BaseTask> readyTasks = GetReadyTasks();
                    StartTasks(readyTasks);
                    CleanUpPendingTasks(readyTasks);
                }
                Task.Delay(SleepPeriod);
            }
        }

        private List<BaseTask> GetReadyTasks()
        {
            List<BaseTask> readyTasks = new List<BaseTask>();

            foreach (var item in pendingTasks)
            {
                if (!runningTasks.Exists(t => t.CollidesWith(item)) && !readyTasks.Exists(t => t.CollidesWith(item)))
                {
                    readyTasks.Add(item);
                }
            }
            return readyTasks;
        }

        private void StartTasks(List<BaseTask> readyTasks)
        {
            foreach (var task in readyTasks)
            {
                task.OnFinished += (s, e) =>
                {
                    lock (mutex)
                    {
                        runningTasks.Remove((BaseTask)s);
                    }
                };
                task.Start();
            }
        }
        private void CleanUpPendingTasks(List<BaseTask> startedTasks)
        {
            foreach (var item in startedTasks)
            {
                pendingTasks.Remove(item);
            }
        }
    }
}
