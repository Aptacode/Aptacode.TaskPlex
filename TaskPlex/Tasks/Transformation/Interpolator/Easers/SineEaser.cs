using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers
{
    public class SineEaser : Easer
    {
        private const double PiOverTwo = Math.PI / 2;

        /// <summary>
        ///     Output accelerates up to 0.5 then decelerates up to 1.0 as the index approaches the total
        /// </summary>
        /// <param name="index"></param>
        /// <param name="total"></param>
        public override double ProgressAt(int index, int total)
        {
            var x = Normalize(index, total);
            return (Math.Sin(x * Math.PI - PiOverTwo) + 1.0) / 2.0;
        }
    }
}