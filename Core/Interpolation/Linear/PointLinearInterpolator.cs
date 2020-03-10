using System.Drawing;
using System.Numerics;

namespace Aptacode.TaskPlex.Interpolation.Linear
{
    public sealed class PointLinearInterpolator : Vec2LinearInterpolator<Point>
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