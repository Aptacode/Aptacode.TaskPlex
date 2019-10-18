using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class LinearGroupTaskEventArgs : BaseTaskEventArgs
    {
    }

    public class SequentialGroupTask : GroupTask
    {
        /// <summary>
        /// Execute the specified tasks sequentially in the order they occur in the input list
        /// </summary>
        /// <param name="tasks"></param>
        public SequentialGroupTask(IEnumerable<BaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }
        /// <summary>
        /// Returns true if the input task collides with any of the groups children
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool CollidesWith(BaseTask item)
        {
            return Tasks.Exists(t => t.CollidesWith(item));
        }

        protected sealed override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
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
            try
            {
                RaiseOnStarted(new LinearGroupTaskEventArgs());

                foreach (var task in Tasks)
                {
                    await task.StartAsync(_cancellationToken).ConfigureAwait(false);
                }

                RaiseOnFinished(new LinearGroupTaskEventArgs());
            }
            catch (TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }
    }
}