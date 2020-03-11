using Aptacode.TaskPlex.Engine;
using Aptacode.TaskPlex.Engine.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.Graphics;
using Aptacode.TaskPlex.Graphics.Nodes.Attribute;
using Aptacode.TaskPlex.Graphics.Nodes.Geometry;
using Aptacode.TaskPlex.Interpolation.Easers;
using System;
using System.Drawing;
using System.Numerics;

namespace Aptacode.TaskPlex.BlazorDemo.Models
{
    public static class SceneLoader
    {
        public static Scene Load()
        {
            var scene = new Scene();

            var _node = new SetColorNode() { Color = Color.Green };
            var _rectangle = new RectangleNode() { Name = "Rect1", Position = new Vector2(10, 10), Size = new Vector2(100, 100) };
            _node.Children.Add(_rectangle);
            scene.Add(_node);

            return scene;
        }


        public static StoryBoard LoadStoryBoard(Scene scene)
        {
            var storyBoard = new StoryBoard(scene);
            var rect = scene["Rect1"] as RectangleNode;

            storyBoard["Load"] = new Vec2Transformation<RectangleNode>(rect, "Position", TimeSpan.FromMilliseconds(2000), Easers.EaseInOutCubic, true, new Vector2(400, 400), new Vector2(0, 0));

            return storyBoard;
        }

    }
}
