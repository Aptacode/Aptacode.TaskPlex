using System;

namespace Aptacode.TaskPlex.Core.Tasks.Transformation.Inerpolator.Easing
{
    public abstract class IEaser
    {
        public abstract double ProgressAt(int index, int count);

        protected double Normalize(int index, int count)
        {
            return (double)index / count;
        }
    }
}
