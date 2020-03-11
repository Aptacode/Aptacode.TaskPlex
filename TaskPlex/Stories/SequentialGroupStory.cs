using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Stories
{
    public class SequentialGroupStory : GroupStory
    {
        private int _finishedTaskCount;

        /// <summary>
        ///     Execute the specified tasks sequentially in the order they occur in the input list
        /// </summary>
        /// <param name="tasks"></param>
        public SequentialGroupStory(List<BaseStory> tasks) : base(TimeSpan.FromMilliseconds(tasks.Aggregate(0,
            (current, task) => current + (int) task.Duration.TotalMilliseconds)), tasks)
        {
        }

        public override void Pause()
        {
            Tasks.ForEach(t => t.Pause());
        }

        public override void Resume()
        {
            Tasks.ForEach(t => t.Resume());
        }

        protected override void Setup()
        {
            _finishedTaskCount = 0;
            foreach (var baseTask in Tasks)
            {
                baseTask.OnFinished += BaseTask_OnFinished;
                baseTask.OnCancelled += BaseTask_OnFinished;
            }
        }

        protected override void Begin()
        {
            Tasks.First().Start(CancellationTokenSource, RefreshRate);
        }

        private void BaseTask_OnFinished(object sender, EventArgs e)
        {
            if (!(sender is BaseStory task))
            {
                return;
            }

            _finishedTaskCount++;

            var nextTaskIndex = Tasks.IndexOf(task) + 1;
            if (nextTaskIndex < Tasks.Count)
            {
                Tasks[nextTaskIndex].Start(CancellationTokenSource, RefreshRate);
            }
        }

        protected override void Cleanup()
        {
            _finishedTaskCount = 0;

            foreach (var baseTask in Tasks)
            {
                baseTask.OnFinished -= BaseTask_OnFinished;
                baseTask.OnCancelled -= BaseTask_OnFinished;
            }
        }

        public override void Update()
        {
            if (IsCancelled)
            {
                Finished();
                return;
            }

            if (!IsRunning())
            {
                return;
            }

            if (_finishedTaskCount >= Tasks.Count)
            {
                Finished();
            }

            Tasks.ForEach(task => task.Update());
        }

        public override void Reset()
        {
            State = TaskState.Paused;
            Cleanup();
            Tasks.ForEach(task => task.Reset());
        }
    }
}