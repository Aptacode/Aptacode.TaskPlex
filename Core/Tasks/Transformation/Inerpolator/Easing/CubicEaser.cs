using System;

namespace Aptacode.TaskPlex.Core.Tasks.Transformation.Inerpolator.Easing
{
    public class CubicInEaser : IEaser
    {
        public override double ProgressAt(int index, int count)
        {
            return Math.Pow(Normalize(index, count), 2);
        }
    }

    public class CubicOutEaser : IEaser
    {
        public override double ProgressAt(int index, int count)
        {
            return Math.Sqrt(Normalize(index, count));
        }
    }
}
