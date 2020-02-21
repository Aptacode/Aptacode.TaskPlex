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

        public override bool Equals(object obj) => obj is ParallelGroupTask task && task.GetHashCode() == GetHashCode();

        protected sealed override TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks)
        {
            return tasks.Select(t => t.Duration)
                .OrderByDescending(t => t.TotalMilliseconds)
                .FirstOrDefault();
        }

        protected override async Task InternalTask()
        {

        }

    }
}