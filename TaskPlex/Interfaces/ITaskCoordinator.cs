using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex.Interfaces
{
    public interface ITaskCoordinator
    {
        void Reset();
        void Pause();
        void Resume();
        void Stop();
        void Start();

        Task Apply(BaseTask task);
        TaskState State { get; }
        IQueryable<BaseTask> GetTasks();
        void Stop(BaseTask task);
        void Pause(BaseTask task);
        void Resume(BaseTask task);

    }
}