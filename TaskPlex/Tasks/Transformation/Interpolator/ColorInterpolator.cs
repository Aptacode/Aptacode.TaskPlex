using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class ColorInterpolator : Interpolator<Color>
    {
        public IEnumerable<Color> Interpolate(Color startValue, Color endValue, int stepCount, Easer easer)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            var componentInterpolator = new DoubleInterpolator();
            var aValues = componentInterpolator.Interpolate(startValue.A, endValue.A, stepCount, easer);
            var rValues = componentInterpolator.Interpolate(startValue.R, endValue.R, stepCount, easer);
            var gValues = componentInterpolator.Interpolate(startValue.G, endValue.G, stepCount, easer);
            var bValues = componentInterpolator.Interpolate(startValue.B, endValue.B, stepCount, easer);

            for (var stepIndex = 1; stepIndex < stepCount; stepIndex++)
            {
                yield return Color.FromArgb((byte) aValues.ElementAt(stepIndex), (byte) rValues.ElementAt(stepIndex),
                    (byte) gValues.ElementAt(stepIndex), (byte) bValues.ElementAt(stepIndex));
            }

            yield return endValue;
        }
    }
}