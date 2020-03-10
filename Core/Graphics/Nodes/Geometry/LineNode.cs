using System.Numerics;

namespace Aptacode.TaskPlex.Graphics.Nodes.Geometry
{
    public class LineNode : GeometryNode
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public override void Update(ICanvas canvas)
        {
            canvas.Apply(this);
            base.Update(canvas);
        }
    }
}