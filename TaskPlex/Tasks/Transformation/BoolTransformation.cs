using System;
using System.Threading.Tasks;
using System.Timers;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public sealed class BoolTransformation<TClass> : PropertyTransformation<TClass, bool> where TClass : class
    {
        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        private BoolTransformation(TClass target,
            string property,
            Func<bool> endValue,
            TimeSpan taskDuration,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            taskDuration,
            refreshRate)
        {

        }

        public static BoolTransformation<T> Create<T>(T target, string property, bool endValue, TimeSpan duration,
            RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new BoolTransformation<T>(target, property, () => endValue, duration, refreshRate);
            }
            catch
            {
                return null;
            }
        }

        private int _tickCount = 0;
        protected override void Setup()
        {

        }

        public override void Dispose()
        {

        }

        protected override async Task InternalTask()
        {
            if (Duration.TotalMilliseconds < 10)
            {
                SetValue(GetEndValue());
                return;
            }

            State = TaskState.Running;
            _tickCount = 0;

            var timer = new Timer((int)RefreshRate);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            while (State != TaskState.Stopped)
            {
                await Task.Delay(1, CancellationToken.Token).ConfigureAwait(false);
            }

            timer.Stop();
            timer.Dispose();

            SetValue(GetEndValue());
        }

        private bool IsRunning()
        {
            return State == TaskState.Running;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsRunning())
            {
                return;
            }

            if (++_tickCount < StepCount)
            {
                return;
            }

            State = TaskState.Stopped;
        }
    }
}