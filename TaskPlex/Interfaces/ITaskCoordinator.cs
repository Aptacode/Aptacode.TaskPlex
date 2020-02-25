using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex.Interfaces
{
    public interface ITaskCoordinator : IDisposable
    {
        void Reset();
        void Pause();
        void Resume();
        void Cancel();
        Task Apply(BaseTask task);
    }
}