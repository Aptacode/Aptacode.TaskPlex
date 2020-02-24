using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class ParallelGroupTask : GroupTask
    {
        private int _endedTaskCount;

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

        protected override void Setup()
        {
            OnCancelled += (s, e) => Tasks.ForEach(task => task.Cancel());

            foreach (var baseTask in Tasks)
            {
                baseTask.OnFinished += (s, e) => { _endedTaskCount++; };
                baseTask.OnCancelled += (s, e) => { _endedTaskCount++; };
            }
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
            Tasks.ForEach(task => task.StartAsync(CancellationToken).ConfigureAwait(false));

            while (_endedTaskCount < Tasks.Count)
            {
                await Task.Delay(10, CancellationToken.Token).ConfigureAwait(false);
            }
        }
    }
}