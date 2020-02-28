using System.Numerics;

namespace Aptacode.TaskPlex.Interpolators.Linear
{
    public abstract class Vec2LinearInterpolator<T> : VectorLinearInterpolator<T, Vector2>
    {
        public override Vector2 Subtract(Vector2 from, Vector2 to)
        {
            return from - to;
        }

        public override Vector2 Normalize(Vector2 from)
        {
            return Vector2.Normalize(from);
        }

        public override float GetLength(Vector2 vector)
        {
            return vector.Length();
        }

        public override Vector2 Add(Vector2 from, Vector2 to)
        {
            return Vector2.Add(from, to);
        }

        public override Vector2 Multiply(Vector2 from, float value)
        {
            return Vector2.Multiply(from, value);
        }
    }
}