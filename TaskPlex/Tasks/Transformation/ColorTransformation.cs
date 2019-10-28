﻿using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class ColorTransformation : PropertyTransformation<Color>
    {
        private readonly object _mutex = new object();
        private Color endValue;
        private bool isRunning = true;
        private readonly ConcurrentQueue<int>
            _aComponentQueue,
            _rComponentQueue,
            _gComponentQueue,
            _bComponentQueue;

        /// <summary>
        ///     Transform a Color property on the target object to the value returned by the given Func<> at intervals specified by
        ///     the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="destinationValue"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public ColorTransformation(object target, string property, Func<Color> destinationValue, Action<Color> setter, TimeSpan taskDuration,
            TimeSpan stepDuration) : base(target, property, destinationValue, setter, taskDuration, stepDuration)
        {
            _aComponentQueue = new ConcurrentQueue<int>();
            _rComponentQueue = new ConcurrentQueue<int>();
            _gComponentQueue = new ConcurrentQueue<int>();
            _bComponentQueue = new ConcurrentQueue<int>();

            Easer = new LinearEaser();
        }

        /// <summary>
        ///     Transform a Color property on the target object to the value returned by the given Func<> at intervals specified by
        ///     the step duration up to the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="destinationValue"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public ColorTransformation(object target, string property, Color destinationValue, TimeSpan taskDuration,
            TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            _aComponentQueue = new ConcurrentQueue<int>();
            _rComponentQueue = new ConcurrentQueue<int>();
            _gComponentQueue = new ConcurrentQueue<int>();
            _bComponentQueue = new ConcurrentQueue<int>();

            Easer = new LinearEaser();
        }

        /// <summary>
        ///     Returns the easing function for this transformation
        /// </summary>
        public Easer Easer { get; set; }

        protected override async Task InternalTask()
        {
            var startValue = GetStartValue();
            endValue = GetEndValue();
            isRunning = true;

            var aComponentInterpolator = new IntInterpolator(startValue.A, endValue.A, Duration, StepDuration);
            var rComponentInterpolator = new IntInterpolator(startValue.R, endValue.R, Duration, StepDuration);
            var gComponentInterpolator = new IntInterpolator(startValue.G, endValue.G, Duration, StepDuration);
            var bComponentInterpolator = new IntInterpolator(startValue.B, endValue.B, Duration, StepDuration);

            aComponentInterpolator.Easer = Easer;
            rComponentInterpolator.Easer = Easer;
            gComponentInterpolator.Easer = Easer;
            bComponentInterpolator.Easer = Easer;

            aComponentInterpolator.OnValueChanged += (s, e) =>
            {
                _aComponentQueue.Enqueue(e.Value);
                ComponentUpdated();
            };

            rComponentInterpolator.OnValueChanged += (s, e) =>
            {
                _rComponentQueue.Enqueue(e.Value);
                ComponentUpdated();
            };

            gComponentInterpolator.OnValueChanged += (s, e) =>
            {
                _gComponentQueue.Enqueue(e.Value);
                ComponentUpdated();
            };

            bComponentInterpolator.OnValueChanged += (s, e) =>
            {
                _bComponentQueue.Enqueue(e.Value);
                ComponentUpdated();
            };

            await Task.WhenAll(
                aComponentInterpolator.StartAsync(CancellationToken),
                rComponentInterpolator.StartAsync(CancellationToken),
                gComponentInterpolator.StartAsync(CancellationToken),
                bComponentInterpolator.StartAsync(CancellationToken)).ContinueWith(o =>
            {
                isRunning = false;
            }).ConfigureAwait(false);
        }

        private void ComponentUpdated()
        {
            if (!isRunning)
            {
                return;
            }

            if (_aComponentQueue.TryPeek(out var newA) &&
                _rComponentQueue.TryPeek(out var newR) &&
                _gComponentQueue.TryPeek(out var newG) &&
                _bComponentQueue.TryPeek(out var newB))
            {
                lock (_mutex)
                {
                    _aComponentQueue.TryDequeue(out _);
                    _rComponentQueue.TryDequeue(out _);
                    _gComponentQueue.TryDequeue(out _);
                    _bComponentQueue.TryDequeue(out _);
                    SetValue(Color.FromArgb(newA, newR, newG, newB));
                }
            }
        }
    }
}