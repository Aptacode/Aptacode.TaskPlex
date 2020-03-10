using System.Numerics;
using System.Windows.Media;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator
{
    public class ColorInterpolator : Vec4LinearInterpolator<Color>
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