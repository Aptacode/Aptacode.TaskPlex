using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class SineEaser : Easer
    {
        private readonly double constant = Math.PI / 2;

        public override double ProgressAt(int index, int count)
        {
            var x = Normalize(index, count);
            return (Math.Sin(x * Math.PI - constant) + 1.0) / 2.0;
        }
    }
}