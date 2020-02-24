using System.Collections.Generic;
using System.Drawing;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolators
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
            var aValues = componentInterpolator.Interpolate(startValue.A, endValue.A, stepCount, easer).GetEnumerator();
            var rValues = componentInterpolator.Interpolate(startValue.R, endValue.R, stepCount, easer).GetEnumerator();
            var gValues = componentInterpolator.Interpolate(startValue.G, endValue.G, stepCount, easer).GetEnumerator();
            var bValues = componentInterpolator.Interpolate(startValue.B, endValue.B, stepCount, easer).GetEnumerator();

            for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                aValues.MoveNext();
                rValues.MoveNext();
                gValues.MoveNext();
                bValues.MoveNext();

                yield return Color.FromArgb((byte) aValues.Current, (byte) rValues.Current, (byte) gValues.Current,
                    (byte) bValues.Current);
            }

            aValues.Dispose();
            rValues.Dispose();
            gValues.Dispose();
            bValues.Dispose();


            yield return endValue;
        }
    }
}