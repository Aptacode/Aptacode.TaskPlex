using System.Numerics;

namespace Aptacode.TaskPlex.Interpolation.Linear
{
    public sealed class Vec2Interpolator : Vec2LinearInterpolator<Vector2>
    {
        public override Vector2 FromVector(Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }

        public override Vector2 ToVector(Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }
    }
}