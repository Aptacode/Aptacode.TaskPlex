namespace Aptacode.TaskPlex.Graphics.Nodes.Transform
{
    public class RotateNode : TransformNode
    {
        public float Angle { get; set; }

        public override void Update(ICanvas canvas)
        {
            canvas.Push();
            canvas.Apply(this);
            base.Update(canvas);
            canvas.Pop();
        }
    }
}