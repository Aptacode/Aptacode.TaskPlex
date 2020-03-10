using System.Drawing;

namespace Aptacode.TaskPlex.Graphics.Nodes.Attribute
{
    public class SetColorNode : SetAttributeNode
    {
        public Color Color { get; set; }

        public override void Update(ICanvas canvas)
        {
            canvas.Push();
            canvas.Apply(this);
            base.Update(canvas);
            canvas.Pop();
        }
    }
}