using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks
{
    public class ParallelGroupTask : GroupTask
    {
        private int _completedTaskCount;

        /// <summary>
        ///     Execute the specified tasks in parallel
        /// </summary>
        public ParallelGroupTask(List<BaseTask> tasks) : base(tasks.Max(task => task.StepCount), tasks)
        {
        }

        public override void Pause()
        {
            Tasks.ForEach(task => task.Pause());
            base.Pause();
        }

        public override void Resume()
        {
            Tasks.ForEach(task => task.Resume());
            base.Resume();
        }

        protected override void Setup()
        {
            _completedTaskCount = 0;
            foreach (var baseTask in Tasks)
            {
                baseTask.OnFinished += IsFinished;
                baseTask.OnCancelled += IsFinished;
            }
        }

        protected override void Begin()
        {
            Tasks.ForEach(task => task.Start(CancellationTokenSource));
        }

        protected override void Cleanup()
        {
            _completedTaskCount = 0;

            foreach (var baseTask in Tasks)
            {
                baseTask.OnFinished -= IsFinished;
                baseTask.OnCancelled -= IsFinished;
            }
        }

        private void IsFinished(object sender, EventArgs args)
        {
            _completedTaskCount++;
        }

        public override void Update()
        {
            if (CancellationTokenSource.IsCancellationRequested)
            {
                Finished();
                return;
            }

            if (!IsRunning())
            {
                return;
            }

            if (_completedTaskCount >= Tasks.Count)
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