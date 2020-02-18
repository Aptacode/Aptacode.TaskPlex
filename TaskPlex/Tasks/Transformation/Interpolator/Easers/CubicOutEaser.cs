using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers
{
    public class CubicOutEaser : Easer
    {
        /// <summary>
        /// Output decelerates from 0->1 as the index approaches the total
        /// </summary>
        /// <param name="index"></param>
        /// <param name="total"></param>
        public override double ProgressAt(int index, int total) => Math.Sqrt(Normalize(index, total));
    }
}