using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode_PropertyTransposer.Transformation.Interpolaton
{
    public abstract class Interpolation<T> : Transformation<T>
    {
        public Interpolation(object target, PropertyInfo property, Func<T> destinationValue, TimeSpan duration) : base(target, property, destinationValue, duration)
        {

        }
        protected abstract T Subtract(T a, T b);
        protected abstract T Divide(T a, int b);
        protected abstract T Add(T a, T b);

        public override void Start()
        {
            Started();

                new TaskFactory().StartNew(() =>
                {
                    //Calculate the total difference between the start and end value
                    T currentValue = GetStartValue();
                    T endValue = GetEndValue();
                    T totalDelta = Subtract(endValue, currentValue);

                    //Calculate the number of increments 
                    int durationMilliseconds = (int)Duration.TotalMilliseconds;
                    int intervalMilliseconds = Interval.TotalMilliseconds >= 1 ? (int)Interval.TotalMilliseconds : 1;
                    int incrementCount = (int)Math.Floor((double)durationMilliseconds / intervalMilliseconds);

                    if (incrementCount > 0)
                    {
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
                            Thread.Sleep(sleepDuration > 0 ? sleepDuration : 0);
                            intervalAdjustmentTimer.Restart();
                        }
                    }
                   
                    //End by updating to the final value
                    UpdateValue(endValue);

                }).ContinueWith((e) =>
                {
                    Finished();
                });
        }
    }
}
