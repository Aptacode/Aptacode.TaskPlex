using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class CubicInEaser : Easer
    {       
        /// <summary>
        /// Output accelerates from 0->1 as the index approaches the count
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override double ProgressAt(int index, int count)
        {
            return Math.Pow(Normalize(index, count), 2);
        }
    }
}