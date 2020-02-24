using System;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex
{
    public interface ITaskCoordinator : IDisposable
    {
        void Reset();
        void Pause();
        void Resume();
        void Apply(BaseTask task);
    }
}