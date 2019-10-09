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
        public Interpolation(object target, PropertyInfo property, Func<T> destinationValue, TimeSpan duration) : base(target, property, destinationValue, duration)
        {
        }

        public Interpolation(object target, PropertyInfo property, T destinationValue, TimeSpan duration) : base(target, property, destinationValue, duration)
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
            int stepDurationMilliSeconds = SteoDuration.TotalMilliseconds >= 1 ? (int)SteoDuration.TotalMilliseconds : 1;
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
            int sleepDuration = (int)(SteoDuration.TotalMilliseconds - stepTimer.ElapsedMilliseconds);
            Task.Delay(sleepDuration > 0 ? sleepDuration : 0);
            stepTimer.Restart();
        }


    }
}
