namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers
{
    public class LinearEaser : Easer
    {
        /// <summary>
        ///     Output approaches 1 at the same rate the index approaches the total
        /// </summary>
        /// <param name="index"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public override double ProgressAt(int index, int total)
        {
            return Normalize(index, total);
        }
    }
}