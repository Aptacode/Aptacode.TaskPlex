using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class ColorTransformationEventArgs : BaseTaskEventArgs
    {
    }

    public class ColorTransformation : PropertyTransformation<Color>
    {
        private readonly Queue<int> _aComponentQueue,
            _rComponentQueue,
            _gComponentQueue,
            _bComponentQueue;

        private readonly object _mutex = new object();

        public ColorTransformation(object target, string property, Func<Color> destinationValue, TimeSpan taskDuration,
            TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            lock (_mutex)
            {
                _aComponentQueue = new Queue<int>();
                _rComponentQueue = new Queue<int>();
                _gComponentQueue = new Queue<int>();
                _bComponentQueue = new Queue<int>();
            }

            Easer = new LinearEaser();
        }

        public ColorTransformation(object target, string property, Color destinationValue, TimeSpan taskDuration,
            TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            lock (_mutex)
            {
                _aComponentQueue = new Queue<int>();
                _rComponentQueue = new Queue<int>();
                _gComponentQueue = new Queue<int>();
                _bComponentQueue = new Queue<int>();
            }

            Easer = new LinearEaser();
        }

        public Easer Easer { get; set; }

        protected override async Task InternalTask()
        {
            try
            {
                RaiseOnStarted(new ColorTransformationEventArgs());

                var startValue = GetStartValue();
                var endValue = GetEndValue();

                var aComponentInterpolator = new IntInterpolator(startValue.A, endValue.A, Duration, StepDuration);
                var rComponentInterpolator = new IntInterpolator(startValue.R, endValue.R, Duration, StepDuration);
                var gComponentInterpolator = new IntInterpolator(startValue.G, endValue.G, Duration, StepDuration);
                var bComponentInterpolator = new IntInterpolator(startValue.B, endValue.B, Duration, StepDuration);

                aComponentInterpolator.SetEaser(Easer);
                rComponentInterpolator.SetEaser(Easer);
                gComponentInterpolator.SetEaser(Easer);
                bComponentInterpolator.SetEaser(Easer);

                aComponentInterpolator.OnValueChanged += (s, e) =>
                {
                    lock (_mutex)
                    {
                        _aComponentQueue.Enqueue(e.Value);
                        ComponentUpdated();
                    }
                };

                rComponentInterpolator.OnValueChanged += (s, e) =>
                {
                    lock (_mutex)
                    {
                        _rComponentQueue.Enqueue(e.Value);
                        ComponentUpdated();
                    }
                };

                gComponentInterpolator.OnValueChanged += (s, e) =>
                {
                    lock (_mutex)
                    {
                        _gComponentQueue.Enqueue(e.Value);
                        ComponentUpdated();
                    }
                };

                bComponentInterpolator.OnValueChanged += (s, e) =>
                {
                    lock (_mutex)
                    {
                        _bComponentQueue.Enqueue(e.Value);
                        ComponentUpdated();
                    }
                };

                await Task.WhenAll(
                    aComponentInterpolator.StartAsync(_cancellationToken),
                    rComponentInterpolator.StartAsync(_cancellationToken),
                    gComponentInterpolator.StartAsync(_cancellationToken),
                    bComponentInterpolator.StartAsync(_cancellationToken));

                RaiseOnFinished(new ColorTransformationEventArgs());
            }
            catch (TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }

        private void ComponentUpdated()
        {
            if (_aComponentQueue.Count > 0 &&
                _rComponentQueue.Count > 0 &&
                _gComponentQueue.Count > 0 &&
                _bComponentQueue.Count > 0)
                SetValue(Color.FromArgb(
                    _aComponentQueue.Dequeue(),
                    _rComponentQueue.Dequeue(),
                    _gComponentQueue.Dequeue(),
                    _bComponentQueue.Dequeue()));
        }
    }
}