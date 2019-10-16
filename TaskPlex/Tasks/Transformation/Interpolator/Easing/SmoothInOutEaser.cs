using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class SmoothInOutEaser : Easer
    {
        private readonly double constant = Math.PI * 2;
        public override double ProgressAt(int index, int count)
        {
            double x = Normalize(index, count);
            return 1 / (1 + (1 / Math.Pow((x / (1 - x)), 2)));
        }
    }
}
