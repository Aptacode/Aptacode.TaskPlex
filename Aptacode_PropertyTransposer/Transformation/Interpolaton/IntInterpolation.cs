using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode_PropertyTransposer.Transformation.Interpolaton
{
    public class IntInterpolation : Interpolation<int>
    {
        public IntInterpolation(object target, PropertyInfo property, Func<int> destinationValue, TimeSpan duration) : base(target, property, destinationValue, duration)
        {

        }

        public override void Start()
        {
            Started();

            new TaskFactory().StartNew(() =>
            {
                int currentValue = GetStartValue();
                int endValue = GetEndValue();
                int distance = endValue - currentValue;

                int stepCount = (int)(Duration.TotalMilliseconds / Interval.TotalMilliseconds);
                int delta = distance / stepCount;

                for (int i = 0; i < stepCount - 1; i++)
                {
                    currentValue += delta;
                    UpdateValue(currentValue);
                    Thread.Sleep(Interval);
                }

                UpdateValue(endValue);

            }).ContinueWith((e) =>
            {
                Finished();
            });
        }
    }
}
