using System;
using Aptacode.TaskPlex.Engine.Interfaces;
using Aptacode.TaskPlex.Graphics;

namespace Aptacode.TaskPlex.Engine
{
    public class CoreEngine
    {
        private readonly ICanvas _canvas;
        private readonly IPlexEngine _plexEngine;
        private readonly Scene _scene;
        private readonly StoryBoard _storyBoard;
        private readonly IUpdater _updater;

        public CoreEngine(IUpdater updater, ICanvas canvas, Scene scene, StoryBoard storyBoard, IPlexEngine plexEngine)
        {
            _updater = updater;
            _canvas = canvas;
            _scene = scene;
            _storyBoard = storyBoard;
            _plexEngine = plexEngine;
            Setup();
        }

        private void Setup()
        {
            _plexEngine.Start();

            _canvas.Setup();
            _updater.OnUpdate += Update;
        }

        private void Update(object sender, EventArgs e)
        {
            _plexEngine.Update();
            _scene.Update(_canvas);
        }

        public void Start()
        {
            _updater.Start();
        }

        public void Apply(string storyName)
        {
            _plexEngine.Apply(_storyBoard[storyName]);
        }
    }
}