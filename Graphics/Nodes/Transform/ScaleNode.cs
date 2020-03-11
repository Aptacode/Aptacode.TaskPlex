using System.Numerics;

namespace Aptacode.TaskPlex.Graphics.Nodes.Transform
{
    public class ScaleNode : TransformNode
    {
        public Vector2 Factor { get; set; }

        public override void Update(ICanvas canvas)
        {
            canvas.Push();
            canvas.Apply(this);
            base.Update(canvas);
            canvas.Pop();
        }
    }
}