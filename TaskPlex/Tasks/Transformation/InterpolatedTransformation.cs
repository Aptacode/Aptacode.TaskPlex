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
        private readonly Timer _timer;
        private SynchronizationContext _context;
        private IEnumerator<TProperty> _interpolationEnumerator;
        private int _interpolationIndex;

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
            _timer = new Timer((int) RefreshRate);
            _timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public Easer Easer { get; set; } = new LinearEaser();

        protected override async Task InternalTask()
        {
            var startValue = GetValue();
            var endValue = GetEndValue();

            _context = SynchronizationContext.Current;
            _interpolationEnumerator =
                _interpolator.Interpolate(startValue, endValue, StepCount, Easer).GetEnumerator();
            State = TaskState.Running;

            //Update First value
            _interpolationIndex = 0;
            _timer.Start();

            while (State == TaskState.Running)
            {
                await Task.Delay(15, CancellationToken.Token).ConfigureAwait(false);
            }

            _timer.Stop();
            _timer.Dispose();
            _interpolationEnumerator.Dispose();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (State != TaskState.Running)
            {
                return;
            }

            if (_interpolationIndex > StepCount)
            {
                State = TaskState.Stopped;
                return;
            }

            UpdateValue(NextValue());
        }

        private TProperty NextValue()
        {
            var value = _interpolationEnumerator.Current;
            _interpolationEnumerator.MoveNext();
            _interpolationIndex++;
            return value;
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