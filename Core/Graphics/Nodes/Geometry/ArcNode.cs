using System.Numerics;

namespace Aptacode.TaskPlex.Graphics.Nodes.Geometry
{
    public class ArcNode : GeometryNode
    {
        public Vector2 Position { get; set; }
        public double Radius { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public bool? IsCounterClockwise { get; set; }

        public override void Update(ICanvas canvas)
        {
            canvas.Apply(this);
            base.Update(canvas);
        }
    }
}