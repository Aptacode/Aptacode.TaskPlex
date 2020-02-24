using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class SequentialGroupTask : GroupTask
    {
        private TaskCoordinator _taskCoordinator;

        private int endedTaskCount;
        /// <summary>
        ///     Execute the specified tasks sequentially in the order they occur in the input list
        /// </summary>
        /// <param name="tasks"></param>
        public SequentialGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }

        protected sealed override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
        {
            return tasks.Aggregate(TimeSpan.Zero, (current, task) => current.Add(task.Duration));
        }

        public override bool Equals(object obj)
        {
            return obj is SequentialGroupTask task && task.GetHashCode() == GetHashCode();
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

                for (var i = 1; i < Tasks.Count; i++)
                {
                    var localIndex = i;

                    Tasks[localIndex - 1].OnFinished += (s, e) => taskCoordinator.Apply(Tasks[localIndex]);
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
            _taskCoordinator.Apply(Tasks[0]);

            while (endedTaskCount < Tasks.Count)
            {
                await Task.Delay(10, CancellationToken.Token).ConfigureAwait(false);
            }
        }
    }
}