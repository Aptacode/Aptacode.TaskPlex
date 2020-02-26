using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex.Tests
{
    public class DummyTask : BaseTask
    {
        private int _tickCount;

        public DummyTask(TimeSpan duration) : base(duration)
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