using System.Drawing;
using System.Numerics;

namespace Aptacode.TaskPlex.Interpolators
{
    public class PointInterpolator : Vec2Interpolator<Point>
    {
        public override Point FromVector(Vector2 value)
        {
            return new Point((int) value.X, (int) value.Y);
        }

        public override Vector2 ToVector(Point value)
        {
            return new Vector2(value.X, value.Y);
        }
    }
}