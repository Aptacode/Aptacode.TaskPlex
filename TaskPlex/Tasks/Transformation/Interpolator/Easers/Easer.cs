using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers
{
    public delegate float EaserFunction(int index, int total);

    public static class Easers
    {
        private const double PiOverTwo = Math.PI / 2;

        public static float Linear(int index, int total)
        {
            return Normalize(index, total);
        }

        public static float CubicOut(int index, int total)
        {
            return (float)Math.Sqrt(Normalize(index, total));
        }
        public static float CubicIn(int index, int total)
        {
            var x = Normalize(index, total);
            return x * x;
        }

        public static float Sine(int index, int total)
        {
            var x = (double)Normalize(index, total);
            return ((float)Math.Sin(x * Math.PI - PiOverTwo) + 1.0f) / 2.0f;
        }

        private static float Normalize(int index, int total)
        {
            if (index < 0)
            {
                return 0;
            }

            if (index >= total || total < 1)
            {
                return 1;
            }

            return index / (float)total;
        }
    }
}