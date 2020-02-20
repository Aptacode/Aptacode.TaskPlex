using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator
{
    public class PointInterpolator : Interpolator<Point>
    {
        public IEnumerable<Point> Interpolate(Point startValue, Point endValue, int stepCount, Easer easer)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            var componentInterpolator = new DoubleInterpolator();
            var xValues = componentInterpolator.Interpolate(startValue.X, endValue.X, stepCount, easer);
            var yValues = componentInterpolator.Interpolate(startValue.Y, endValue.Y, stepCount, easer);

            for (var stepIndex = 1; stepIndex < stepCount; stepIndex++)
            {
                yield return new Point(xValues.ElementAt(stepIndex), yValues.ElementAt(stepIndex));
            }

            yield return endValue;
        }
    }
}