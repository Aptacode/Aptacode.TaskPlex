using System;
using System.Collections.Generic;
using System.Linq;
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

        protected InterpolatedTransformation(TClass target,
            string property,
            TimeSpan duration,
            Interpolator<TProperty> interpolator,
            EaserFunction easerFunction = null,
            params TProperty[] values) : base(target,
            property,
            duration,
            values)
        {
            _interpolator = interpolator;
            Easer = easerFunction ?? Easers.Linear;
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public EaserFunction Easer { get; set; }

        protected override void Setup()
        {
            var startValue = GetValue();
            var endValue = Values[0];
            _interpolationEnumerator = _interpolator.Interpolate(startValue, endValue, StepCount, Easer).ToList()
                .GetEnumerator();
        }

        protected override void Begin()
        {
        }

        protected override void Cleanup()
        {
            _interpolationEnumerator?.Dispose();
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

            if (_interpolationEnumerator?.MoveNext() == true)
            {
                SetValue(_interpolationEnumerator.Current);
            }
            else
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