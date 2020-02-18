namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class IntInterpolator : Interpolator<int>
    {
        protected override int Add(int a, int b) => a + b;

        protected override int Divide(int a, int incrementCount)
        {
            if(incrementCount <= 1)
            {
                return a;
            }

            return a / incrementCount;
        }

        protected override int Subtract(int a, int b) => a - b;
    }
}