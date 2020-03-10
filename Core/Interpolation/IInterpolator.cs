using System.Collections.Generic;
using Aptacode.TaskPlex.Interpolation.Easers;

namespace Aptacode.TaskPlex.Interpolation
{
    public interface IInterpolator<T>
    {
        IEnumerable<T> Interpolate(int stepCount, EaserFunction easer, params T[] points);
    }
}