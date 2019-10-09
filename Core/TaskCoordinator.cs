using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aptacode.Core.Tasks;

namespace Aptacode.Core
{
    public class TaskCoordinator
    {
        public TimeSpan SleepPeriod { get; set; }

        private readonly List<BaseTask> pendingTasks;
        private readonly List<BaseTask> runningTasks;
        private static readonly Object mutex = new Object();
        public bool IsRunning { get; set; }

        public TaskCoordinator(TimeSpan sleepPeriod)
        {
            pendingTasks = new List<BaseTask>();
            runningTasks = new List<BaseTask>();
            IsRunning = false;
            SleepPeriod = sleepPeriod;
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

        private async Task run()
        {
            while (IsRunning)
            {
                lock (mutex)
                {
                    List<BaseTask> readyTasks = getReadyTasks();
                    cleanUpPendingTasks(readyTasks);
                    startTasks(readyTasks);
                }

                await Task.Delay(SleepPeriod);
            }
        }

        private List<BaseTask> getReadyTasks()
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

        private void startTasks(List<BaseTask> readyTasks)
        {
            foreach (var task in readyTasks)
            {
                BaseTask localTask = task;
                runningTasks.Add(localTask);

                localTask.OnFinished += (s, e) =>
                {
                    lock (mutex)
                    {
                        runningTasks.Remove((BaseTask)localTask);
                    }
                };

                localTask.StartAsync();
            }
        }
        private void cleanUpPendingTasks(List<BaseTask> startedTasks)
        {
            foreach (var item in startedTasks)
            {
                pendingTasks.Remove(item);
            }
        }
    }
}
