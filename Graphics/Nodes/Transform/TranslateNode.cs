using System.Numerics;

namespace Aptacode.TaskPlex.Graphics.Nodes.Transform
{
    public class TranslateNode : TransformNode
    {
        public Vector2 Translation { get; set; }

        public override void Update(ICanvas canvas)
        {
            canvas.Push();
            canvas.Apply(this);
            base.Update(canvas);
            canvas.Pop();
        }
    }
}