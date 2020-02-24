using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class ParallelGroupTask : GroupTask
    {
        private TaskCoordinator _taskCoordinator;

        private int endedTaskCount;

        /// <summary>
        ///     Execute the specified tasks in parallel
        /// </summary>
        public ParallelGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }

        public override bool Equals(object obj)
        {
            return obj is ParallelGroupTask task && task.GetHashCode() == GetHashCode();
        }

        protected sealed override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
        {
            return tasks.Select(t => t.Duration)
                .OrderByDescending(t => t.TotalMilliseconds)
                .FirstOrDefault();
        }

        internal override async Task InternalTask(TaskCoordinator taskCoordinator)
        {
            _taskCoordinator = taskCoordinator;

            if (Tasks.Count > 0)
            {
                OnCancelled += (s, e) =>
                {
                    foreach (var task1 in Tasks)
                    {
                        task1.Cancel();
                    }
                };

                foreach (var baseTask in Tasks)
                {
                    baseTask.OnFinished += (s, e) => { endedTaskCount++; };
                    baseTask.OnCancelled += (s, e) => { endedTaskCount++; };
                }


                await StartAsync().ConfigureAwait(false);
            }
        }

        public override void Pause()
        {
            Tasks.ForEach(t => t.Pause());
        }

        public override void Resume()
        {
            Tasks.ForEach(t => t.Resume());
        }

        protected override async Task InternalTask()
        {
            Tasks.ForEach(_taskCoordinator.Apply);

            while (endedTaskCount < Tasks.Count)
            {
                await Task.Delay(10, CancellationToken.Token).ConfigureAwait(false);
            }
        }
    }
}