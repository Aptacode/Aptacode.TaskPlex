using System.Drawing;
using System.Numerics;

namespace Aptacode.TaskPlex.Interpolators
{
    public class ColorInterpolator : Vec4Interpolator<Color>
    {
        public override Vector4 ToVector(Color value)
        {
            return new Vector4(value.A, value.R, value.G, value.B);
        }

        public override Color FromVector(Vector4 value)
        {
            return Color.FromArgb((byte) value.X, (byte) value.Y, (byte) value.Z, (byte) value.W);
        }
    }
}