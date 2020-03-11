using System.Linq;
using Aptacode.TaskPlex.Stories;

namespace Aptacode.TaskPlex.Interfaces
{
    public interface IStoryManager
    {
        void Apply(BaseStory story);
        IQueryable<BaseStory> GetAll();
        void Stop(BaseStory story);
        void Pause(BaseStory story);
        void Resume(BaseStory story);

        void Stop();
        void Pause();
        void Resume();

        void Update();
    }
}