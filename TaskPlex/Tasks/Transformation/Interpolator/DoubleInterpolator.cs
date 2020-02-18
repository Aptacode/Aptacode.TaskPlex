namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class DoubleInterpolator : Interpolator<double>
    {
        protected override double Add(double a, double b)
        {
            return a + b;
        }

        protected override double Divide(double a, int incrementCount)
        {
            if (incrementCount <= 1)
            {
                return a;
            }

            return a / incrementCount;
        }

        protected override double Subtract(double a, double b)
        {
            return a - b;
        }
    }
}