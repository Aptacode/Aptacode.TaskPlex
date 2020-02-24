using System.Collections.Generic;
using System.Windows.Media;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators.Easers;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator
{
    public class ColorInterpolator : Interpolator<Color>
    {
        public IEnumerable<Color> Interpolate(Color startValue, Color endValue, int stepCount, EaserFunction easer)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            var componentInterpolator = new DoubleInterpolator();
            var aValueIterator = componentInterpolator.Interpolate(startValue.A, endValue.A, stepCount, easer)
                .GetEnumerator();
            var rValueIterator = componentInterpolator.Interpolate(startValue.R, endValue.R, stepCount, easer)
                .GetEnumerator();
            var gValueIterator = componentInterpolator.Interpolate(startValue.G, endValue.G, stepCount, easer)
                .GetEnumerator();
            var bValueIterator = componentInterpolator.Interpolate(startValue.B, endValue.B, stepCount, easer)
                .GetEnumerator();

            for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                aValueIterator.MoveNext();
                rValueIterator.MoveNext();
                gValueIterator.MoveNext();
                bValueIterator.MoveNext();

                yield return Color.FromArgb((byte) aValueIterator.Current, (byte) rValueIterator.Current,
                    (byte) gValueIterator.Current, (byte) bValueIterator.Current);
            }

            aValueIterator.Dispose();
            rValueIterator.Dispose();
            gValueIterator.Dispose();
            bValueIterator.Dispose();

            yield return endValue;
        }
    }
}