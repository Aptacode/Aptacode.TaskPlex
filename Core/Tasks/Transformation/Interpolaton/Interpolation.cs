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

    public abstract class Interpolation<T> : PropertyTransformation<T>
    {
        private Stopwatch stepTimer;
        public Interpolation(object target, PropertyInfo property, Func<T> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            stepTimer = new Stopwatch();
        }

        public Interpolation(object target, PropertyInfo property, T destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
            stepTimer = new Stopwatch();
        }

        protected abstract T Subtract(T a, T b);
        protected abstract T Divide(T a, int b);
        protected abstract T Add(T a, T b);

        public override async Task StartAsync()
        {
            RaiseOnStarted(new InterpolationEventArgs());

            int StepCount = GetNumberOfSteps();

            if (StepCount > 0)
            {
                await InterpolateAsync(StepCount);
            }

            //End by updating to the final value
            UpdateValue(GetEndValue());

            RaiseOnFinished(new InterpolationEventArgs());

        }

        private int GetNumberOfSteps()
        {
            int taskDurationMilliSeconds = (int)TaskDuration.TotalMilliseconds;
            int stepDurationMilliSeconds = StepDuration.TotalMilliseconds >= 1 ? (int)StepDuration.TotalMilliseconds : 1;
            return (int)Math.Floor((double)taskDurationMilliSeconds / stepDurationMilliSeconds);
        }

        private async Task InterpolateAsync(int stepCount)
        {
            T currentValue = GetStartValue();
            T endValue = GetEndValue();
            T totalDelta = Subtract(endValue, currentValue);
            T incrementDelta = Divide(totalDelta, stepCount);

            stepTimer.Restart();
            for (int stepIndex = 0; stepIndex < stepCount - 1; stepIndex++)
            {
                currentValue = Add(currentValue, incrementDelta);
                UpdateValue(currentValue);
                await delayAsync(stepIndex);
            }
        }

        private async Task delayAsync(int currentStep)
        {
            int millisecondsAhead = (int)(StepDuration.TotalMilliseconds * currentStep - stepTimer.ElapsedMilliseconds);
            if(millisecondsAhead > 0)
            {
                await Task.Delay(millisecondsAhead);
            }
        }
    }
}
