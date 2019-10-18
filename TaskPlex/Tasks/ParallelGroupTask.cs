using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.EventArgs;

namespace Aptacode.TaskPlex.Tasks
{
    public class ParallelGroupTask : GroupTask
    {
        /// <summary>
        ///     Execute the specified tasks in parallel
        /// </summary>
        public ParallelGroupTask(List<IBaseTask> tasks) : base(tasks)
        {
            Duration = GetTotalDuration(Tasks);
        }

        /// <summary>
        ///     Returns true if the input task collides with any of the groups children
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool CollidesWith(IBaseTask item)
        {
            return Tasks.Exists(t => t.CollidesWith(item));
        }

        protected sealed override TimeSpan GetTotalDuration(List<IBaseTask> tasks)
        {
            return tasks.Select(t => t.Duration).OrderByDescending(t => t.TotalMilliseconds).FirstOrDefault();
        }

        protected override async Task InternalTask()
        {
            try
            {
                
                RaiseOnStarted(new ParallelGroupTaskEventArgs());

                await Task.WhenAll(Tasks.Select(task => task.StartAsync(_cancellationToken))).ConfigureAwait(false);

                RaiseOnFinished(new ParallelGroupTaskEventArgs());
            }
            catch (TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }
    }
}