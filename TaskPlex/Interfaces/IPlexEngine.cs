using System.Linq;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex.Interfaces
{
    public interface IPlexEngine
    {
        TaskState State { get; }
        void Reset();
        void Pause();
        void Resume();
        void Stop();
        void Start();

        void Apply(BaseTask task);
        IQueryable<BaseTask> GetTasks();
        void Stop(BaseTask task);
        void Pause(BaseTask task);
        void Resume(BaseTask task);
    }
}