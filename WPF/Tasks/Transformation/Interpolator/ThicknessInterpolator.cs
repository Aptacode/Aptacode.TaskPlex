using System.Collections.Generic;
using System.Windows;
using Aptacode.TaskPlex.Interpolators;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator
{
    public class ThicknessInterpolator : Interpolator<Thickness>
    {
        public IEnumerable<Thickness> Interpolate(Thickness startValue, Thickness endValue, int stepCount,
            EaserFunction easer)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            var componentInterpolator = new DoubleInterpolator();
            var topInterpolator = componentInterpolator.Interpolate(startValue.Top, endValue.Top, stepCount, easer)
                .GetEnumerator();
            var bottomInterpolator = componentInterpolator
                .Interpolate(startValue.Bottom, endValue.Bottom, stepCount, easer)
                .GetEnumerator();
            var leftInterpolator = componentInterpolator.Interpolate(startValue.Left, endValue.Left, stepCount, easer)
                .GetEnumerator();
            var rightInterpolator = componentInterpolator
                .Interpolate(startValue.Right, endValue.Right, stepCount, easer)
                .GetEnumerator();

            for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                topInterpolator.MoveNext();
                bottomInterpolator.MoveNext();
                leftInterpolator.MoveNext();
                rightInterpolator.MoveNext();

                yield return new Thickness(leftInterpolator.Current, topInterpolator.Current,
                    rightInterpolator.Current, bottomInterpolator.Current);
            }

            topInterpolator.Dispose();
            bottomInterpolator.Dispose();
            leftInterpolator.Dispose();
            rightInterpolator.Dispose();

            yield return endValue;
        }
    }
}