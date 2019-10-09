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
        public Interpolation(object target, PropertyInfo property, Func<T> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public Interpolation(object target, PropertyInfo property, T destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        protected abstract T Subtract(T a, T b);
        protected abstract T Divide(T a, int b);
        protected abstract T Add(T a, T b);

        public override void Start()
        {
            RaiseOnStarted(new InterpolationEventArgs());

            new TaskFactory().StartNew(() =>
                {
                    int StepCount = GetNumberOfSteps();

                    if (StepCount > 0)
                    {
                        Interpolate(StepCount);
                    }
                   
                    //End by updating to the final value
                    UpdateValue(GetEndValue());

                }).ContinueWith((e) =>
                {
                    RaiseOnFinished(new InterpolationEventArgs());
                });
        }

        private int GetNumberOfSteps()
        {
            int taskDurationMilliSeconds = (int)TaskDuration.TotalMilliseconds;
            int stepDurationMilliSeconds = StepDuration.TotalMilliseconds >= 1 ? (int)StepDuration.TotalMilliseconds : 1;
            return (int)Math.Floor((double)taskDurationMilliSeconds / stepDurationMilliSeconds);
        }

        private void Interpolate(int stepCount)
        {
            T currentValue = GetStartValue();
            T endValue = GetEndValue();
            T totalDelta = Subtract(endValue, currentValue);
            T incrementDelta = Divide(totalDelta, stepCount);

            //Adjust how long to sleep each step based on how long it took to update the value and the step duration
            Stopwatch stepTimer = new Stopwatch();
            stepTimer.Start();

            for (int i = 0; i < stepCount - 1; i++)
            {
                currentValue = Add(currentValue, incrementDelta);
                UpdateValue(currentValue);
                Delay(stepTimer);
            }
        }

        private void Delay(Stopwatch stepTimer)
        {
            int sleepDuration = (int)(StepDuration.TotalMilliseconds - stepTimer.ElapsedMilliseconds);
            if(sleepDuration > 0)
                Task.Delay(sleepDuration);
            stepTimer.Restart();
        }
    }
}
