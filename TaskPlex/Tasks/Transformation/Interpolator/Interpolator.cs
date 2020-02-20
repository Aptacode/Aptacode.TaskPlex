using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public interface Interpolator<T>
    {
        IEnumerable<T> Interpolate(T startValue, T endValue, int stepCount, Easer easer);
    }
}