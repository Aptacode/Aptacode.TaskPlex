using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class SequentialGroupTask : GroupTask
    {
        /// <summary>
        ///     Execute the specified tasks sequentially in the order they occur in the input list
        /// </summary>
        /// <param name="tasks"></param>
        public SequentialGroupTask(List<BaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }

        protected sealed override TimeSpan GetTotalDuration(List<BaseTask> tasks)
        {
            var totalDuration = TimeSpan.Zero;
            foreach (var task in tasks)
            {
                totalDuration = totalDuration.Add(task.Duration);
            }

            return totalDuration;
        }

        protected override async Task InternalTask()
        {
            foreach (var task in Tasks)
            {
                await task.StartAsync(CancellationToken).ConfigureAwait(false);
            }
        }
    }
}