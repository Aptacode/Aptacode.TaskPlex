using System.Numerics;

namespace Aptacode.TaskPlex.Graphics.Nodes.Geometry
{
    public class RectangleNode : GeometryNode
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public override void Update(ICanvas canvas)
        {
            canvas.Apply(this);
            base.Update(canvas);
        }
    }
}