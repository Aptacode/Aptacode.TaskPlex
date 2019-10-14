using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class CubicInEaser : Easer
    {
        public override double ProgressAt(int index, int count)
        {
            return Math.Pow(Normalize(index, count), 2);
        }
    }

    public class CubicOutEaser : Easer
    {
        public override double ProgressAt(int index, int count)
        {
            return Math.Sqrt(Normalize(index, count));
        }
    }
}
