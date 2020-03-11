using System.Collections.Generic;
using Aptacode.TaskPlex.Engine.Tasks;
using Aptacode.TaskPlex.Graphics;

namespace Aptacode.TaskPlex.Engine
{
    public class StoryBoard
    {
        private readonly Scene _scene;
        private readonly Dictionary<string, BaseTask> _taskDictionary;

        public StoryBoard(Scene scene)
        {
            _scene = scene;
            _taskDictionary = new Dictionary<string, BaseTask>();
        }

        public BaseTask this[string storyName]
        {
            get
            {
                _taskDictionary.TryGetValue(storyName, out var value);

                return value;
            }
            set => _taskDictionary[storyName] = value;
        }

        public void Add(string storyName, BaseTask task)
        {
            _taskDictionary.Add(storyName, task);
        }
    }
}