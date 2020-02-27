using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks
{
    public class WaitTask : BaseTask
    {
        private int _tickCount;

        /// <summary>
        ///     Wait for a specified amount of time
        /// </summary>
        /// <param name="stepCount"></param>
        public WaitTask(int stepCount) : base(stepCount)
        {
        }

        protected override void Setup()
        {
            _tickCount = 0;
        }

        protected override void Begin()
        {
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
                Finished();
            }
        }

        public override void Reset()
        {
            State = TaskState.Paused;
            Cleanup();
        }
    }
}