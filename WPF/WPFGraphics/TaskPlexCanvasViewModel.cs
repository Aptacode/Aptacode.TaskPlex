using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Aptacode.TaskPlex.Graphics;
using Aptacode.TaskPlex.Graphics.Nodes.Attribute;
using Aptacode.TaskPlex.Graphics.Nodes.Geometry;
using Aptacode.TaskPlex.Graphics.Nodes.Transform;

namespace WPFGraphics
{
    public class TaskPlexCanvasViewModel : FrameworkElement, ICanvas
    {
        private readonly Stack<Brush> _brushStack;
        private readonly DrawingVisual _visual;
        private Brush _brush;
        private DrawingContext _drawingContext;

        public TaskPlexCanvasViewModel()
        {
            _brushStack = new Stack<Brush>();
            _visual = new DrawingVisual();
            AddVisualChild(_visual);
        }

        protected override int VisualChildrenCount => _visual != null ? 1 : 0;

        public void Update(Scene scene)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                _brushStack.Clear();
                _brush = Brushes.Black;

                // Retrieve the DrawingContext in order to create new drawing content.
                _drawingContext = _visual.RenderOpen();

                scene.Root.Update(this);

                // Persist the drawing content.
                _drawingContext.Close();
            });
        }

        public void Push()
        {
            _brushStack.Push(_brush);
        }

        public void Pop()
        {
            _drawingContext.Pop();
            _brush = _brushStack.Pop();
        }

        public void Apply(RectangleNode node)
        {
            _drawingContext.DrawRectangle(_brush, null,
                new Rect(new Point(node.Position.X, node.Position.Y), new Size(node.Size.X, node.Size.Y)));
        }

        public void Apply(SetColorNode node)
        {
            _drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, 1000, 1000)));
            _brush = new SolidColorBrush(Color.FromArgb(node.Color.A, node.Color.R, node.Color.G, node.Color.B));
        }

        public void Apply(TranslateNode node)
        {
            _drawingContext.PushTransform(new TranslateTransform(node.Translation.X, node.Translation.Y));
        }

        public void Apply(RotateNode rotateNode)
        {
            _drawingContext.PushTransform(new RotateTransform(rotateNode.Angle));
        }

        public void Apply(ScaleNode scaleNode)
        {
            _drawingContext.PushTransform(new ScaleTransform(scaleNode.Factor.X, scaleNode.Factor.Y));
        }

        public void Apply(ArcNode arcNode)
        {
        }

        public void Apply(LineNode lineNode)
        {
        }

        public void Setup()
        {
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visual;
        }
    }
}