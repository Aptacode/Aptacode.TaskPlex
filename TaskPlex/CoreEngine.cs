using System;
using Aptacode.TaskPlex.Graphics;
using Aptacode.TaskPlex.Interfaces;

namespace Aptacode.TaskPlex
{
    public class CoreEngine
    {
        private readonly ICanvas _canvas;
        private readonly Scene _scene;
        private readonly StoryBoard _storyBoard;
        private readonly IStoryManager _storyManager;
        private readonly IUpdater _updater;

        public CoreEngine(IUpdater updater, ICanvas canvas, Scene scene, StoryBoard storyBoard,
            IStoryManager storyManager)
        {
            _updater = updater;
            _canvas = canvas;
            _scene = scene;
            _storyBoard = storyBoard;
            _storyManager = storyManager;
            Setup();
        }

        private void Setup()
        {
            _canvas.Setup();
            _updater.OnUpdate += Update;
        }

        private void Update(object sender, EventArgs e)
        {
            _storyManager.Update();
            _canvas.Update(_scene);
        }

        public void Start()
        {
            _updater.Start();
        }

        public void Apply(string storyName)
        {
            _storyManager.Apply(_storyBoard[storyName]);
        }
    }
}