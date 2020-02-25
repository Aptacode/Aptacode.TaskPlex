using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class ParallelGroupTask : GroupTask
    {
        /// <summary>
        ///     Execute the specified tasks in parallel
        /// </summary>
        public ParallelGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }

        protected sealed override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
        {
            return tasks.Select(t => t.Duration)
                .OrderByDescending(t => t.TotalMilliseconds)
                .FirstOrDefault();
        }

        public override void Pause()
        {
            Tasks.ForEach(task => task.Pause());
        }

        public override void Resume()
        {
            Tasks.ForEach(task => task.Resume());
        }

        protected override async Task InternalTask()
        {
            var tasks = Tasks.Select(baseTask => baseTask.StartAsync(CancellationTokenSource)).ToArray();

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}