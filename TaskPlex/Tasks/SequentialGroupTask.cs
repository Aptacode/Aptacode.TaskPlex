using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class SequentialGroupTask : GroupTask
    {
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
            foreach (var baseTask in Tasks)
            {
                await baseTask.StartAsync(CancellationTokenSource, RefreshRate).ConfigureAwait(false);
            }
        }
    }
}