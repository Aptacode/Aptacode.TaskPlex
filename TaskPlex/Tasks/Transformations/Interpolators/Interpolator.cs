using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformations.Interpolators.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformations.Interpolators
{
    public interface Interpolator<T>
    {
        IEnumerable<T> Interpolate(T startValue, T endValue, int stepCount, EaserFunction easer);
    }
}