namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing
{
    public class LinearEaser : Easer
    {
        public override double ProgressAt(int index, int count)
        {
            return Normalize(index, count);
        }
    }
}