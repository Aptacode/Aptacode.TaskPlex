using System;

namespace Aptacode.TaskPlex.Core.Tasks.Transformation.Inerpolator.Easing
{
    public class LinearEaser : IEaser
    {
        public override double ProgressAt(int index, int count)
        {
            return Normalize(index, count);
        }
    }
}
