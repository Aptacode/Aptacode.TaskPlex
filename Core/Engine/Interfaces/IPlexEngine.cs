using System.Linq;
using Aptacode.TaskPlex.Engine.Enums;
using Aptacode.TaskPlex.Engine.Tasks;

namespace Aptacode.TaskPlex.Engine.Interfaces
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