using System;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers
{
    public delegate float EaserFunction(int index, int total);

    public static class Easers
    {
        private const double PiOverTwo = Math.PI / 2;

        public static float Linear(int index, int total)
        {
            return Percent(index, total);
        }

        public static float Elastic(int index, int total)
        {
            var x = (double)Percent(index, total);
            var p = 0.3;
            return (float)(Math.Pow(2, -10 * x) * Math.Sin((x - p / 4) * (2 * Math.PI) / p) + 1);
        }

        public static float EaseInQuad(int index, int total)
        {
            var x = Percent(index, total);
            return x * x;
        }
        public static float EaseOutQuad(int index, int total)
        {
            var x = Percent(index, total);
            return x * (2 - x);
        }

        public static float EaseInOutQuad(int index, int total)
        {
            var x = Percent(index, total);
            return x < .5 ? 2 * x * x : -1 + (4 - 2 * x) * x;
        }

        public static float EaseInCubic(int index, int total)
        {
            var x = Percent(index, total);
            return x * x * x;
        }

        public static float EaseOutCubic(int index, int total)
        {
            var x = Percent(index, total);
            return (--x) * x * x + 1;
        }

        public static float EaseInOutCubic(int index, int total)
        {
            var x = Percent(index, total);
            return x < .5 ? 4 * x * x * x : (x - 1) * (2 * x - 2) * (2 * x - 2) + 1;
        }

        public static float EaseInQuart(int index, int total)
        {
            var x = Percent(index, total);
            return x * x * x * x;
        }

        public static float EaseOutQuart(int index, int total)
        {
            var x = Percent(index, total);
            return 1 - (--x) * x * x * x;
        }

        public static float EaseInOutQuart(int index, int total)
        {
            var x = Percent(index, total);
            return x < .5 ? 8 * x * x * x * x : 1 - 8 * (--x) * x * x * x;
        }

        public static float EaseInQuint(int index, int total)
        {
            var x = Percent(index, total);
            return x * x * x * x * x;
        }

        public static float EaseOutQuint(int index, int total)
        {
            var x = Percent(index, total);
            return 1 + (--x) * x * x * x * x;
        }

        public static float EaseInOutQuint(int index, int total)
        {
            var x = Percent(index, total);
            return x < .5 ? 16 * x * x * x * x * x : 1 + 16 * (--x) * x * x * x * x;
        }


        private static float Percent(int index, int total)
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