using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCoordinator.Tasks.Transformation.Interpolaton
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

        public override event EventHandler<BaseTaskEventArgs> OnStarted;
        public override event EventHandler<BaseTaskEventArgs> OnFinished;

        protected abstract T Subtract(T a, T b);
        protected abstract T Divide(T a, int b);
        protected abstract T Add(T a, T b);

        public override void Start()
        {
            OnStarted?.Invoke(this, new InterpolationEventArgs());

            new TaskFactory().StartNew(() =>
                {
                    //Calculate the total difference between the start and end value
                    T endValue = GetEndValue();
                    int incrementCount = GetIncrementCount();

                    if (incrementCount > 0)
                    {
                        RunInterpolation(incrementCount);
                    }
                   
                    //End by updating to the final value
                    UpdateValue(endValue);

                }).ContinueWith((e) =>
                {
                    OnFinished?.Invoke(this, new InterpolationEventArgs());
                });
        }

        private void RunInterpolation(int incrementCount)
        {
            T currentValue = GetStartValue();
            T endValue = GetEndValue();
            T totalDelta = Subtract(endValue, currentValue);

            //If there are more then one increments calculate the incremental difference
            T incrementDelta = Divide(totalDelta, incrementCount);

            //Adjust how long to sleep each iteration based on how long it took to increment the value
            Stopwatch intervalAdjustmentTimer = new Stopwatch();
            intervalAdjustmentTimer.Start();

            for (int i = 0; i < incrementCount - 1; i++)
            {
                currentValue = Add(currentValue, incrementDelta);
                UpdateValue(currentValue);

                int sleepDuration = (int)(Interval.TotalMilliseconds - intervalAdjustmentTimer.ElapsedMilliseconds);
                Task.Delay(sleepDuration > 0 ? sleepDuration : 0);
                intervalAdjustmentTimer.Restart();
            }
        }

        private int GetIncrementCount()
        {
            //Calculate the number of increments 
            int durationMilliseconds = (int)Duration.TotalMilliseconds;
            int intervalMilliseconds = Interval.TotalMilliseconds >= 1 ? (int)Interval.TotalMilliseconds : 1;
            return (int)Math.Floor((double)durationMilliseconds / intervalMilliseconds);
        }

    }
}
