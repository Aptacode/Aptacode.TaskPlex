using System;
using System.Collections.Generic;
using System.Linq;
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

        internal override Task InternalTask(TaskCoordinator taskCoordinator)
        {
            throw new NotImplementedException();
        }

        protected override async Task InternalTask()
        {
            var endedTaskCount = 0;
            foreach (var baseTask in Tasks)
            {
                baseTask.OnFinished += (s, e) => { endedTaskCount++; };
                baseTask.OnCancelled += (s, e) => { endedTaskCount++; };
            }

            while (endedTaskCount < Tasks.Count)
            {
                await Task.Delay(10, CancellationToken.Token).ConfigureAwait(false);
            }
        }

        public override bool Equals(object obj) => obj is SequentialGroupTask task && task.GetHashCode() == GetHashCode();
    }
}