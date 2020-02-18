using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers
{
    public class CubicInEaser : Easer
    {
        /// <summary>
        ///     Output accelerates from 0->1 as the index approaches the total
        /// </summary>
        /// <param name="index"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public override double ProgressAt(int index, int total)
        {
            return Math.Pow(Normalize(index, total), 2);
        }
    }
}