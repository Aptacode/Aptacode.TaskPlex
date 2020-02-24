using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;
using Timer = System.Timers.Timer;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class InterpolatedTransformation<TClass, TProperty> : PropertyTransformation<TClass, TProperty>
        where TClass : class
    {
        private readonly Interpolator<TProperty> _interpolator;
        private SynchronizationContext _context;
        private IEnumerator<TProperty> _interpolationEnumerator;

        protected InterpolatedTransformation(TClass target,
            string property,
            Func<TProperty> endValue,
            TimeSpan duration,
            Interpolator<TProperty> interpolator,
            RefreshRate refreshRate = RefreshRate.Normal) : base(target,
            property,
            endValue,
            duration,
            refreshRate)
        {
            _interpolator = interpolator;
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public EaserFunction Easer { get; set; } = Easers.Linear;

        private Timer _timer;

        protected override void Setup()
        {
            var startValue = GetValue();
            var endValue = GetEndValue();

            _context = SynchronizationContext.Current;
            _interpolationEnumerator = _interpolator.Interpolate(startValue, endValue, StepCount, Easer).GetEnumerator();
            _timer = new Timer((int)RefreshRate);
            _timer.Elapsed += Timer_Elapsed;
        }

        public override void Dispose()
        {
            _timer.Dispose();
            _interpolationEnumerator.Dispose();
        }

        protected override async Task InternalTask()
        {
            _timer.Start();

            while (State != TaskState.Stopped)
            {
                await Task.Delay(1, CancellationToken.Token).ConfigureAwait(false);
            }

            _timer.Stop();
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

            if (!_interpolationEnumerator.MoveNext())
            {
                State = TaskState.Stopped;
                return;
            }

            UpdateValue(_interpolationEnumerator.Current);
        }

        private void UpdateValue(TProperty value)
        {
            if (_context == null)
            {
                SetValue(value);
            }
            else
            {
                _context.Post(delegate { SetValue(value); }, null);
            }
        }
    }
}