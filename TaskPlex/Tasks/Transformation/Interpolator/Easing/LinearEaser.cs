namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class LinearEaser : Easer
    {
        /// <summary>
        /// Output approaches 1 at the same rate the index approaches the count
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override double ProgressAt(int index, int count) => Normalize(index, count);
    }
}