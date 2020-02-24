using System.Collections.Generic;
using System.Windows;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators.Easers;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator
{
    public class PointInterpolator : Interpolator<Point>
    {
        public IEnumerable<Point> Interpolate(Point startValue, Point endValue, int stepCount, EaserFunction easer)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            var componentInterpolator = new DoubleInterpolator();
            var xValueIterator = componentInterpolator.Interpolate(startValue.X, endValue.X, stepCount, easer)
                .GetEnumerator();
            var yValueIterator = componentInterpolator.Interpolate(startValue.Y, endValue.Y, stepCount, easer)
                .GetEnumerator();

            for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                xValueIterator.MoveNext();
                yValueIterator.MoveNext();

                yield return new Point(xValueIterator.Current, yValueIterator.Current);
            }

            xValueIterator.Dispose();
            yValueIterator.Dispose();


            yield return endValue;
        }
    }
}