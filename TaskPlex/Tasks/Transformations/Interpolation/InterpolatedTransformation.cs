using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolation
{
    public class InterpolatedTransformation<TClass, TProperty> : PropertyTransformation<TClass, TProperty>
        where TClass : class
    {
        private readonly Interpolator<TProperty> _interpolator;
        private IEnumerator<TProperty> _interpolationEnumerator;
        private bool _interpolatorHasValues;

        protected InterpolatedTransformation(TClass target,
            string property,
            Func<TProperty> endValue,
            TimeSpan duration,
            Interpolator<TProperty> interpolator,
            EaserFunction easerFunction = null) : base(target,
            property,
            endValue,
            duration)
        {
            _interpolator = interpolator;
            Easer = easerFunction ?? Easers.Linear;
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public EaserFunction Easer { get; set; }

        protected override async Task InternalTask()
        {
            var startValue = GetValue();
            var endValue = GetEndValue();
            _interpolationEnumerator = _interpolator.Interpolate(startValue, endValue, _stepCount, Easer).ToList()
                .GetEnumerator();

            _interpolatorHasValues = true;

            while (_interpolatorHasValues && !CancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }

            _interpolationEnumerator?.Dispose();
        }

        public override void Update()
        {
            if (!IsRunning() || _interpolationEnumerator == null)
            {
                return;
            }

            _interpolatorHasValues = _interpolationEnumerator.MoveNext();

            if (_interpolatorHasValues)
            {
                SetValue(_interpolationEnumerator.Current);
            }
        }
    }
}