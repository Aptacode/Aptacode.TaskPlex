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
        private IEnumerator<TProperty> _keyFrameEnumerator;
        private int _keyFrameIndex;
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
            _keyFrameIndex = 0;
            _interpolationEnumerator = GetNextInterpolator();
        }

        private IEnumerator<TProperty> GetNextInterpolator()
        {
            if (_keyFrameIndex < Values.Length)
            {
                return _interpolationEnumerator = _interpolator.Interpolate(GetValue(), Values[_keyFrameIndex++], StepCount / Values.Length, Easer).ToList()
                    .GetEnumerator();
            }

            return null;
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
                _interpolationEnumerator = GetNextInterpolator();
                if (_interpolationEnumerator?.MoveNext() == true)
                {
                    SetValue(_interpolationEnumerator.Current);
                }
                else
                {
                    Finished();
                }
            }
        }

        public override void Reset()
        {
            State = TaskState.Paused;
            Cleanup();
        }
    }
}