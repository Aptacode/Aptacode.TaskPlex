using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows;
using Aptacode.Interpolatr;
using Aptacode.TaskPlex;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Graphics;
using Aptacode.TaskPlex.Graphics.Nodes.Attribute;
using Aptacode.TaskPlex.Graphics.Nodes.Geometry;
using Aptacode.TaskPlex.Stories;
using Aptacode.TaskPlex.Stories.Transformations.Interpolation;
using Microsoft.Extensions.Logging.Abstractions;
using WPFGraphics;

namespace WPFDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskPlexCanvasViewModel _canvas;
        private CoreEngine _engine;
        private Scene _scene;
        private StoryBoard _storyBoard;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _canvas = TaskPlexCanvas.DataContext as TaskPlexCanvasViewModel;
            Setup();
            Start();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _engine.Apply("MoveAll");
        }

        public void Setup()
        {
            CreateScene();
            CreateStoryBoard();

            _engine = new CoreEngine(new WpfUpdater(), _canvas, _scene, _storyBoard,
                new StoryManager(new NullLoggerFactory(), RefreshRate.Normal));
        }

        public void Start()
        {
            _engine.Start();
        }

        public void CreateScene()
        {
            _scene = new Scene();

            var fillNode = new SetColorNode {Color = Color.Green};


            for (var i = 0; i < 10; i++)
            {
                fillNode.Children.Add(
                    new RectangleNode { Name = $"Rect{i}", Position = new Vector2(i * 50, 50), Size = new Vector2(40, 40) });
            }

            _scene.Root.Children.Add(fillNode);
        }

        public void CreateStoryBoard()
        {
            _storyBoard = new StoryBoard(_scene);

            var storyList = new List<BaseStory>();

            for (var i = 0; i < 10; i++)
            {
                var rect = _scene[$"Rect{i}"] as RectangleNode;

                var move = new Vec2Transformation<RectangleNode>(rect, "Position",
                    TimeSpan.FromMilliseconds(40 + i * 10), DefaultEasers.EaseInOutCubic, true, new Vector2(rect.Position.X, 200));

                storyList.Add(move);
            }

            for (var i = 9; i >= 0; i--)
            {
                var rect = _scene[$"Rect{i}"] as RectangleNode;

                var move = new Vec2Transformation<RectangleNode>(rect, "Position",
                    TimeSpan.FromMilliseconds(40 + (10 - i) * 10), DefaultEasers.EaseInOutCubic, true, new Vector2(rect.Position.X, rect.Position.Y));

                storyList.Add(move);
            }

            _storyBoard["MoveAll"] = StoryBuilder.Sequential(storyList.ToArray());
        }
    }
}