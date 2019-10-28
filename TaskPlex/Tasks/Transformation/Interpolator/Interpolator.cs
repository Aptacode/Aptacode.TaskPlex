using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.EventArgs;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public abstract class Interpolator<T> : BaseTask
    {
        private readonly Stopwatch _stepTimer;

        protected Interpolator(T startValue, T endValue, TimeSpan duration, TimeSpan intervalDuration) : base(duration)
        {
            _stepTimer = new Stopwatch();
            Easer = new LinearEaser();

            CurrentValue = StartValue = startValue;
            EndValue = endValue;
            IntervalDuration = intervalDuration;
        }

        /// <summary>
        ///     Returns the Easing function
        /// </summary>
        public Easer Easer { get; set; }

        public TimeSpan IntervalDuration { get; set; }
        private T StartValue { get; }
        private T CurrentValue { get; set; }
        private T EndValue { get; }
        public event EventHandler<InterpolationValueChangedEventArgs<T>> OnValueChanged;

        protected abstract T Subtract(T a, T b);
        protected abstract T Add(T a, T b);
        protected abstract T Divide(T a, int incrementCount);

        protected override async Task InternalTask()
        {
            await new TaskFactory(CancellationToken.Token).StartNew(() =>
            {
                _stepTimer.Restart();
                var stepCount = GetStepCount();
                var incrementValue = GetIncrementValue(StartValue, EndValue, stepCount);
                var incrementIndex = 0;

                for (var stepIndex = 1; stepIndex < stepCount; stepIndex++)
                {
                    var nextIncrementIndex = GetNextIncrementIndex(stepIndex, stepCount);

                    UpdateValue(incrementIndex, incrementValue, nextIncrementIndex);
                    incrementIndex = nextIncrementIndex;

                    DelayAsync(stepIndex).Wait();
                }

                OnValueChanged?.Invoke(this, new InterpolationValueChangedEventArgs<T>(EndValue));

            }).ConfigureAwait(false);
        }

        private int GetStepCount()
        {
            var taskDurationMilliSeconds = (int) Duration.TotalMilliseconds;
            var stepDurationMilliSeconds =
                IntervalDuration.TotalMilliseconds >= 1 ? (int) IntervalDuration.TotalMilliseconds : 1;
            return (int) Math.Floor((double) taskDurationMilliSeconds / stepDurationMilliSeconds);
        }

        private T GetIncrementValue(T startValue, T endValue, int stepCount)
        {
            var difference = Subtract(endValue, startValue);

            if (stepCount <= 1)
            {
                return difference;
            }


            return Divide(difference, stepCount);
        }

        private int GetNextIncrementIndex(int stepIndex, int stepCount)
        {
            return (int) Math.Floor(Easer.ProgressAt(stepIndex, stepCount) * stepCount);
        }

        private void UpdateValue(int incrementIndex, T incrementValue, int nextIncrementIndex)
        {
            while (incrementIndex < nextIncrementIndex)
            {
                incrementIndex++;
                CurrentValue = Add(CurrentValue, incrementValue);
            }

            OnValueChanged?.Invoke(this, new InterpolationValueChangedEventArgs<T>(CurrentValue));
        }

        private async Task DelayAsync(int currentStep)
        {
            var millisecondsAhead =
                (int) (IntervalDuration.TotalMilliseconds * currentStep - _stepTimer.ElapsedMilliseconds);
            //the Task.Delay function will only accuratly sleep for >8ms
            if (millisecondsAhead > 8)
            {
                await Task.Delay(millisecondsAhead).ConfigureAwait(false);
            }
        }
    }
}