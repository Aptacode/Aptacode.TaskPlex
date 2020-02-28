using System.Collections.Generic;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Interfaces
{
    public interface IInterpolator<T>
    {
        IEnumerable<T> Interpolate(int stepCount, EaserFunction easer, params T[] points);
    }
}