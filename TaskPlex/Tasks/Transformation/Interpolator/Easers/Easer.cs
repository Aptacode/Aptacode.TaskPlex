namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers
{
    public abstract class Easer
    {
        /// <summary>
        ///     Returns a value between 0 and 1 depending on the current index and total
        /// </summary>
        /// <param name="index"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public abstract double ProgressAt(int index, int total);

        protected static double Normalize(int index, int total)
        {
            if (index < 0)
            {
                return 0;
            }

            if (index >= total || total < 1)
            {
                return 1;
            }

            return index / (double) total;
        }
    }
}