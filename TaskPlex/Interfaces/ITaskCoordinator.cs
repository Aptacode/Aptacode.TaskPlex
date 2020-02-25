using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex.Interfaces
{
    public interface ITaskCoordinator : IDisposable
    {
        void Reset();
        void Pause();
        void Resume();
        void CancelAll();
        Task Apply(BaseTask task);
        TaskState State { get; }
    }
}