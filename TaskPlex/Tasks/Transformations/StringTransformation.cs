using System;
using System.Threading.Tasks;
using System.Timers;

namespace Aptacode.TaskPlex.Tasks.Transformations
{
    public sealed class StringTransformation<TClass> : PropertyTransformation<TClass, string> where TClass : class
    {
        private int _tickCount;

        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        private StringTransformation(TClass target,
            string property,
            Func<string> endValue,
            TimeSpan taskDuration,
            RefreshRate refreshRate) : base(target,
            property,
            endValue,
            taskDuration,
            refreshRate)
        {
        }

        public static StringTransformation<T> Create<T>(T target, string property, string endValue, TimeSpan duration,
            RefreshRate refreshRate) where T : class
        {
            return StringTransformation<T>.Create(target, property, () => endValue, duration, refreshRate);
        }

        public static StringTransformation<T> Create<T>(T target, string property, Func<string> endValue,
            TimeSpan duration, RefreshRate refreshRate = RefreshRate.Normal) where T : class
        {
            try
            {
                return new StringTransformation<T>(target, property, endValue, duration, refreshRate);
            }
            catch
            {
                return null;
            }
        }

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

            var timer = new Timer((int) RefreshRate);
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