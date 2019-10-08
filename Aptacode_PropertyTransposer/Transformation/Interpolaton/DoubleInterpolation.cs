using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode_PropertyTransposer.Transformation.Interpolaton
{
    public class DoubleInterpolation : Interpolation<double>
    {
        public DoubleInterpolation(object target, PropertyInfo property, Func<double> destinationValue, TimeSpan duration) : base(target, property, destinationValue, duration)
        {

        }

        public override void Start()
        {
            Started();

            new TaskFactory().StartNew(() =>
            {
                double currentValue = GetStartValue();
                double endValue = GetEndValue();
                double distance = endValue - currentValue;

                int stepCount = (int)(Duration.TotalMilliseconds / Interval.TotalMilliseconds);
                double delta = distance / stepCount;

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
