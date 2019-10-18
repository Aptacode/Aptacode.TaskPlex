using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class CubicOutEaser : Easer
    {
        /// <summary>
        ///     Output decelerates from 0->1 as the index approaches the count
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public override double ProgressAt(int index, int count)
        {
            return Math.Sqrt(Normalize(index, count));
        }
    }
}