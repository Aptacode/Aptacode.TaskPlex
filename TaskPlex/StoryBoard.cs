using System.Collections.Generic;
using Aptacode.TaskPlex.Graphics;
using Aptacode.TaskPlex.Stories;

namespace Aptacode.TaskPlex
{
    public class StoryBoard
    {
        private readonly Scene _scene;
        private readonly Dictionary<string, BaseStory> _storyDictionary;

        public StoryBoard(Scene scene)
        {
            _scene = scene;
            _storyDictionary = new Dictionary<string, BaseStory>();
        }

        public BaseStory this[string storyName]
        {
            get
            {
                _storyDictionary.TryGetValue(storyName, out var value);

                return value;
            }
            set => _storyDictionary[storyName] = value;
        }

        public void Add(string storyName, BaseStory story)
        {
            _storyDictionary.Add(storyName, story);
        }
    }
}