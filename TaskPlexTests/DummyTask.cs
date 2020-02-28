using System;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex.Tests
{
    public class DummyTask : BaseTask
    {
        private int _tickCount;

        public DummyTask(TimeSpan duration) : base(duration)
        {
        }

        public bool HasSetup { get; set; }
        public bool HasStarted { get; set; }
        public bool HasCanceled => CancellationTokenSource.IsCancellationRequested;
        public bool HasFinished { get; set; }

        protected override void Setup()
        {
            _tickCount = 0;
            HasSetup = true;
        }

        protected override void Begin()
        {
            HasStarted = true;
        }

        protected override void Cleanup()
        {
            _tickCount = 0;
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

            if (++_tickCount >= StepCount)
            {
                HasFinished = true;
                Finished();
            }
        }

        public override void Reset()
        {
        }
    }
}