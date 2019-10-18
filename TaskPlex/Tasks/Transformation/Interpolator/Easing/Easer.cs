namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public abstract class Easer
    {
        /// <summary>
        ///     Returns a value between 0 and 1 depending on the current index and count
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public abstract double ProgressAt(int index, int count);

        protected static double Normalize(int index, int count)
        {
            if (count <= 1 || index > count)
            {
                return 1;
            }
            else
            {
                return index / (double) count;
            }
        }
    }
}