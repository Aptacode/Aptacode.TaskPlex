using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class SineEaser : Easer
    {
        private static readonly double PiOverTwo = Math.PI / 2;

        /// <summary>
        ///     Output accelerates up to 0.5 then decelerates up to 1.0 as the index approaches the count
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public override double ProgressAt(int index, int count)
        {
            var x = Normalize(index, count);
            return (Math.Sin(x * Math.PI - PiOverTwo) + 1.0) / 2.0;
        }
    }
}