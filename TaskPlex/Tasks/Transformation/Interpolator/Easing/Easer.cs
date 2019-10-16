namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public abstract class Easer
    {
        public abstract double ProgressAt(int index, int count);

        protected static double Normalize(int index, int count)
        {
            return (double)index / (double)count;
        }
    }
}
