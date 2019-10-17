using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class InterpolationEventArgs : BaseTaskEventArgs
    {
    }

    public class InterpolationValueChangedEventArgs<T> : InterpolationEventArgs
    {
        public T Value { get; set; }

        public InterpolationValueChangedEventArgs(T value)
        {
            Value = value;
        }
    }
    public abstract class Interpolator<T> : BaseTask
    {
        public event EventHandler<InterpolationValueChangedEventArgs<T>> OnValueChanged;

        public Easer Easer { get; set; }

        private readonly Stopwatch _stepTimer;
        private T StartValue { get; set; }
        private T CurrentValue { get; set; }
        private T EndValue { get; set; }
        public TimeSpan IntervalDuration { get; set; }
        protected Interpolator(T startValue, T endValue, TimeSpan duration, TimeSpan intervalDuration) : base(duration)
        {
            _stepTimer = new Stopwatch();
            Easer = new LinearEaser();

            CurrentValue = StartValue = startValue;
            EndValue = endValue;
            IntervalDuration = intervalDuration;

        }

        public void SetEaser(Easer easer)
        {
            Easer = easer;
        }

        public override bool CollidesWith(BaseTask item)
        {
            return false;
        }

        protected abstract T Subtract(T a, T b);
        protected abstract T Add(T a, T b);
        protected abstract T Divide(T a, int incrementCount);
        protected override async Task InternalTask()
        {
            try
            {
                if (_cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                RaiseOnStarted(new InterpolationEventArgs());

                _stepTimer.Restart();
                await InterpolateAsync().ConfigureAwait(false);
                _stepTimer.Stop();
                OnValueChanged?.Invoke(this, new InterpolationValueChangedEventArgs<T>(EndValue));

                RaiseOnFinished(new InterpolationEventArgs());
            }
            catch (TaskCanceledException)
            {
                RaiseOnCancelled();
            }
        }

        private async Task InterpolateAsync()
        {
            int stepCount = GetStepCount();
            T incrementValue = GetIncrementValue(StartValue, EndValue, stepCount);
            int incrementIndex = 0;
            
            for (int stepIndex = 1; stepIndex < stepCount; stepIndex++)
            {
                if (_cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                int nextIncrementIndex = GetNextIncrementIndex(stepIndex, stepCount);

                UpdateValue(incrementIndex, incrementValue, nextIncrementIndex);
                incrementIndex = nextIncrementIndex;

                await DelayAsync(stepIndex);
            }
        }
        private int GetStepCount()
        {
            int taskDurationMilliSeconds = (int)Duration.TotalMilliseconds;
            int stepDurationMilliSeconds = IntervalDuration.TotalMilliseconds >= 1 ? (int)IntervalDuration.TotalMilliseconds : 1;
            return (int)Math.Floor((double)taskDurationMilliSeconds / stepDurationMilliSeconds);
        }

        private T GetIncrementValue(T startValue, T endValue, int stepCount)
        {
            T difference = Subtract(endValue, startValue);

            if (stepCount <= 1)
                return difference;

            return Divide(difference, stepCount);
        }

        private int GetNextIncrementIndex(int stepIndex, int stepCount)
        {
            return (int)Math.Floor(Easer.ProgressAt(stepIndex, stepCount) * stepCount);
        }

        private void UpdateValue(int incrementIndex, T incrementValue, int nextIncrementIndex)
        {
            if (_cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            while (incrementIndex < nextIncrementIndex)
            {
                incrementIndex++;
                CurrentValue = Add(CurrentValue, incrementValue);
            }
            OnValueChanged?.Invoke(this, new InterpolationValueChangedEventArgs<T>(CurrentValue));
        }

        private async Task DelayAsync(int currentStep)
        {
            int millisecondsAhead = (int)(IntervalDuration.TotalMilliseconds * currentStep - _stepTimer.ElapsedMilliseconds);
            //the Task.Delay function will only accuratly sleep for >8ms
            if (millisecondsAhead > 8)
            {
                await Task.Delay(millisecondsAhead, _cancellationToken.Token).ConfigureAwait(false);
            }
        }
    }
}
