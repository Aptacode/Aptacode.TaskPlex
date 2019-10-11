using Aptacode.TaskPlex.Core.Tasks.Transformation.Inerpolator.Easing;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks.Transformations.Interpolation
{
    public class InterpolationEventArgs : BaseTaskEventArgs
    {
        public InterpolationEventArgs()
        {

        }
    }
    public enum Comparison
    {
        Greater, Equal, Less
    }

    public abstract class Interpolator<T> : PropertyTransformation<T>
    {
        public IEaser Easer { get; set; }
        private Stopwatch stepTimer;
        public Interpolator(object target, string property, Func<T> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            stepTimer = new Stopwatch();
            Easer = new LinearEaser();
        }

        public Interpolator(object target, string property, T destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            stepTimer = new Stopwatch();
            Easer = new LinearEaser();
        }

        public void SetEaser(IEaser easer)
        {
            Easer = easer;
        }

        protected abstract T Subtract(T a, T b);
        protected abstract T Add(T a, T b);
        protected abstract T Divide(T a, int incrementCount);
        protected abstract Comparison Compare(T a, T b);


        private T startValue { get; set; }
        private T currentValue { get; set; }
        private T endValue { get; set; }

        public override async Task StartAsync()
        {
            RaiseOnStarted(new InterpolationEventArgs());

            LoadInitialValues();

            stepTimer.Restart();

            if (Compare(startValue, endValue) != Comparison.Equal)
            {
                await InterpolateAsync();
            }
            else
            {
                await Task.Delay(TaskDuration);
            }

            SetValue(endValue);
            stepTimer.Stop();

            RaiseOnFinished(new InterpolationEventArgs());
        }

        private void LoadInitialValues()
        {
            currentValue = startValue = GetStartValue();
            endValue = GetEndValue();
        }

        private async Task InterpolateAsync()
        {
            int stepCount = GetStepCount();
            T incrementValue = GetIncrementValue(startValue, endValue, stepCount);
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
            int taskDurationMilliSeconds = (int)TaskDuration.TotalMilliseconds;
            int stepDurationMilliSeconds = StepDuration.TotalMilliseconds >= 1 ? (int)StepDuration.TotalMilliseconds : 1;
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
            if (nextIncrementIndex > incrementIndex)
            {
                while (incrementIndex < nextIncrementIndex)
                {
                    incrementIndex++;
                    currentValue = Add(currentValue, incrementValue);
                }
                SetValue(currentValue);
            }
        }

        private async Task delayAsync(int currentStep)
        {
            int millisecondsAhead = (int)(StepDuration.TotalMilliseconds * currentStep - stepTimer.ElapsedMilliseconds);
            if (millisecondsAhead > 0)
            {
                await Task.Delay(millisecondsAhead);
            }
        }
    }
}
