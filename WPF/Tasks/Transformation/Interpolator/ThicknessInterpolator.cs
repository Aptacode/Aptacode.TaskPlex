using System.Numerics;
using System.Windows;
using Aptacode.TaskPlex.Interpolators;

namespace Aptacode.TaskPlex.WPF.Tasks.Transformation.Interpolator
{
    public class ThicknessInterpolator : Vec4Interpolator<Thickness>
    {
        public override Vector4 ToVector(Thickness value)
        {
            return new Vector4((float) value.Left, (float) value.Top, (float) value.Right, (float) value.Bottom);
        }

        public override Thickness FromVector(Vector4 value)
        {
            return new Thickness(value.X, value.Y, value.Z, value.W);
        }
    }
}