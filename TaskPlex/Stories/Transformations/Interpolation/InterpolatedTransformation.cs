using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Interpolatr;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Stories.Transformations.Interpolation
{
    public class InterpolatedTransformation<TClass, TProperty> : PropertyTransformation<TClass, TProperty>
        where TClass : class
    {
        private readonly ILinearInterpolator<TProperty> _interpolator;
        private IEnumerator<TProperty> _interpolationEnumerator;

        protected InterpolatedTransformation(TClass target,
            string property,
            TimeSpan duration,
            ILinearInterpolator<TProperty> interpolator,
            EaserFunction easerFunction = null,
            bool useStartValue = true,
            params TProperty[] values) : base(target,
            property,
            duration,
            useStartValue,
            values)
        {
            _interpolator = interpolator;
            Easer = easerFunction ?? DefaultEasers.Linear;
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public EaserFunction Easer { get; set; }

        protected override void Setup()
        {
            var points = new List<TProperty>();
            if (UseStartValue)
            {
                points.Add(GetValue());
            }

            points.AddRange(Values);
            _interpolationEnumerator =
                _interpolator.Interpolate(StepCount, Easer, points.ToArray()).ToList().GetEnumerator();
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
            if (IsCancelled)
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