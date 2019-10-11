using Aptacode.TaskPlex.Core.Tasks.Transformation.Inerpolator.Easing;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks.Transformations.Interpolation
{
    public class InterpolationEventArgs : BaseTaskEventArgs
    {
        public InterpolationEventArgs()
        {

        }
    }

    public class InterpolationValueChangedEventArgs<T> : InterpolationEventArgs
    {
        public T Value { get; set; }

        public InterpolationValueChangedEventArgs(T value)
        {
            Value = value;
        }
    }

    public enum Comparison
    {
        Greater, Equal, Less
    }

    public abstract class Interpolator<T> : BaseTask
    {
        public event EventHandler<InterpolationValueChangedEventArgs<T>> OnValueChanged;

        public IEaser Easer { get; set; }
        private Stopwatch stepTimer;
        private T StartValue { get; set; }
        private T CurrentValue { get; set; }
        private T EndValue { get; set; }
        public TimeSpan IntervalDuration { get; set; }
        public Interpolator(T startValue, T endValue, TimeSpan duration, TimeSpan intervalDuration) : base(duration)
        {
            stepTimer = new Stopwatch();
            Easer = new LinearEaser();

            CurrentValue = StartValue = startValue;
            EndValue = endValue;
            IntervalDuration = intervalDuration;

        }

        public void SetEaser(IEaser easer)
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
        protected abstract Comparison Compare(T a, T b);

        public override async Task StartAsync()
        {
            RaiseOnStarted(new InterpolationEventArgs());

            stepTimer.Restart();

            await InterpolateAsync();

            stepTimer.Stop();
            OnValueChanged?.Invoke(this, new InterpolationValueChangedEventArgs<T>(EndValue));

            RaiseOnFinished(new InterpolationEventArgs());
        }

        private async Task InterpolateAsync()
        {
            int stepCount = GetStepCount();
            T incrementValue = GetIncrementValue(StartValue, EndValue, stepCount);
            int incrementIndex = 0;
            
            for (int stepIndex = 1; stepIndex < stepCount; stepIndex++)
            {
                int nextIncrementIndex = GetNextIncrementIndex(stepIndex, stepCount);

                UpdateValue(incrementIndex, incrementValue, nextIncrementIndex);
                incrementIndex = nextIncrementIndex;

                await delayAsync(stepIndex);
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
            return Divide(difference, stepCount);
        }

        private int GetNextIncrementIndex(int stepIndex, int stepCount)
        {
            return (int)Math.Floor(Easer.ProgressAt(stepIndex, stepCount) * stepCount);
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

        private async Task delayAsync(int currentStep)
        {
            int millisecondsAhead = (int)(IntervalDuration.TotalMilliseconds * currentStep - stepTimer.ElapsedMilliseconds);
            //the Task.Delay function will only accuratly sleep for >8ms
            if (millisecondsAhead > 8)
            {
                await Task.Delay(millisecondsAhead);
            }
        }
    }
}
