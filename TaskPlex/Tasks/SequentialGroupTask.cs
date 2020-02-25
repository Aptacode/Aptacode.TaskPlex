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

        protected override void Setup()
        {
            if (Tasks.Count <= 0)
            {
                return;
            }

            OnCancelled += (s, e) => Tasks.ForEach(task => task.Cancel());

            for (var i = 1; i < Tasks.Count; i++)
            {
                var localIndex = i;

                Tasks[localIndex - 1].OnCancelled += (s, e) => StartTask(Tasks[localIndex]);
                Tasks[localIndex - 1].OnFinished += (s, e) => StartTask(Tasks[localIndex]);
            }

            Tasks.Last().OnCancelled += (s, e) => _endedTaskCount++;
            Tasks.Last().OnFinished += (s, e) => _endedTaskCount++;
        }

        private void StartTask(BaseTask task)
        {
            _endedTaskCount++;
            task.StartAsync(CancellationTokenSource).ConfigureAwait(false);
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
            await Tasks[0].StartAsync(CancellationTokenSource).ConfigureAwait(false);

            while (_endedTaskCount < Tasks.Count)
            {
                await Task.Delay(10, CancellationTokenSource.Token).ConfigureAwait(false);
            }
        }
    }
}