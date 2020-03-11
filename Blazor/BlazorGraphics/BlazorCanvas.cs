using System.Threading.Tasks;
using Aptacode.TaskPlex.Graphics;
using Aptacode.TaskPlex.Graphics.Nodes.Attribute;
using Aptacode.TaskPlex.Graphics.Nodes.Geometry;
using Aptacode.TaskPlex.Graphics.Nodes.Transform;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;

namespace Aptacode.TaskPlex.BlazorGraphics
{
    public class BlazorCanvas : ICanvas
    {
        private readonly BECanvasComponent _canvasReference;
        private Canvas2DContext _canvas2DContext;

        public BlazorCanvas(BECanvasComponent canvasReference)
        {
            _canvasReference = canvasReference;
        }

        public void Push()
        {
            _canvas2DContext.SaveAsync();
        }

        public void Pop()
        {
            _canvas2DContext.RestoreAsync();
        }

        public void Apply(RectangleNode node)
        {
            _canvas2DContext.FillRectAsync(node.Position.X, node.Position.Y, node.Size.X, node.Size.Y);
        }

        public void Apply(SetColorNode node)
        {
            _canvas2DContext.SetFillStyleAsync(node.Color.Name.ToLower());
        }

        public void Apply(TranslateNode node)
        {
            _canvas2DContext.TranslateAsync(node.Translation.X, node.Translation.Y);
        }

        public void Apply(RotateNode rotateNode)
        {
            _canvas2DContext.RotateAsync(rotateNode.Angle);
        }

        public void Apply(ScaleNode scaleNode)
        {
            _canvas2DContext.ScaleAsync(scaleNode.Factor.X, scaleNode.Factor.Y);
        }

        public void Apply(ArcNode arcNode)
        {
            _canvas2DContext.ArcAsync(arcNode.Position.X, arcNode.Position.Y, arcNode.Radius, arcNode.StartAngle,
                arcNode.EndAngle, arcNode.IsCounterClockwise);
        }

        public void Apply(LineNode lineNode)
        {
            _canvas2DContext.BeginPathAsync();
            _canvas2DContext.MoveToAsync(lineNode.Start.X, lineNode.Start.Y);
            _canvas2DContext.LineToAsync(lineNode.End.X, lineNode.End.Y);
            _canvas2DContext.StrokeAsync();
        }


        public void Setup()
        {
            new TaskFactory().StartNew(async () => { _canvas2DContext = await _canvasReference.CreateCanvas2DAsync(); })
                .Wait();
        }

        public void Update(Scene scene)
        {
            scene.Root.Update(this);
        }
    }
}