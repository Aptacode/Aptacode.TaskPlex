using System;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks
{
    public class RepeatTask : BaseTask
    {
        private int _repeatCount;

        public RepeatTask(BaseTask child, int repeatRepeatCount) : base(child.StepCount * repeatRepeatCount)
        {
            Child = child;
            RepeatCount = repeatRepeatCount;
        }

        public BaseTask Child { get; }
        public int RepeatCount { get; set; }

        public override void Pause()
        {
            Child.Pause();
            base.Pause();
        }

        public override void Resume()
        {
            Child.Resume();
            base.Resume();
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

            if (_repeatCount >= RepeatCount)
            {
                Finished();
            }

            Child.Update();
        }

        protected override void Setup()
        {
            _repeatCount = 0;
            Child.OnFinished += Child_OnFinished;
        }

        protected override void Begin()
        {
            Child.Start(CancellationTokenSource);
        }

        private void Child_OnFinished(object sender, EventArgs e)
        {
            if (++_repeatCount < RepeatCount)
            {
                Child.Start(CancellationTokenSource);
            }
        }

        protected override void Cleanup()
        {
            _repeatCount = 0;
            Child.OnFinished -= Child_OnFinished;
        }

        public override void Reset()
        {
            State = TaskState.Paused;
            Cleanup();
            Child.Reset();
        }
    }
}