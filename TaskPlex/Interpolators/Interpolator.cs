using System.Collections.Generic;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Interpolators
{
    public interface Interpolator<T>
    {
        IEnumerable<T> Interpolate(T startValue, T endValue, int stepCount, EaserFunction easer);
    }
}