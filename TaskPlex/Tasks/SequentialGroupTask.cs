using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class SequentialGroupTask : GroupTask
    {
        private int _endedTaskCount;

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

        protected override void Setup()
        {
            if (Tasks.Count <= 0)
            {
                return;
            }

            OnCancelled += (s, e) =>
            {
                foreach (var task1 in Tasks)
                {
                    task1.Cancel();
                }
            };

            foreach (var baseTask in Tasks)
            {
                baseTask.OnFinished += (s, e) => { _endedTaskCount++; };
                baseTask.OnCancelled += (s, e) => { _endedTaskCount++; };
            }

            for (var i = 1; i < Tasks.Count; i++)
            {
                var localIndex = i;

                Tasks[localIndex - 1].OnFinished += (s, e) =>
                    Tasks[localIndex].StartAsync(CancellationToken).ConfigureAwait(false);
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
            await Tasks[0].StartAsync(CancellationToken).ConfigureAwait(false);

            while (_endedTaskCount < Tasks.Count)
            {
                await Task.Delay(10, CancellationToken.Token).ConfigureAwait(false);
            }
        }
    }
}