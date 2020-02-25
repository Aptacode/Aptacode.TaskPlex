using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class WaitTask : BaseTask
    {
        private int _tickCount;

        /// <summary>
        ///     Wait for a specified amount of time
        /// </summary>
        /// <param name="duration"></param>
        public WaitTask(TimeSpan duration) : base(duration)
        {
        }

        protected override async Task InternalTask()
        {
            _tickCount = 0;

            while (_tickCount < _stepCount && !CancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(1).ConfigureAwait(false);
            }
        }

        public override void Update()
        {
            if (!IsRunning())
            {
                return;
            }

            _tickCount++;
        }
    }
}