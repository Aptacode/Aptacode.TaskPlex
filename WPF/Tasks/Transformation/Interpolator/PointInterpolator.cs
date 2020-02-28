using System.Numerics;
using System.Windows;
using Aptacode.TaskPlex.Interpolators.Linear;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator
{
    public class PointInterpolator : Vec2LinearInterpolator<Point>
    {
        public override Point FromVector(Vector2 value)
        {
            return new Point((int) value.X, (int) value.Y);
        }

        public override Vector2 ToVector(Point value)
        {
            return new Vector2((float) value.X, (float) value.Y);
        }
    }
}