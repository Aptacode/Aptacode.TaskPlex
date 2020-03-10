using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Aptacode.TaskPlex.Graphics
{
    public interface ICanvas
    {
        void Push();
        void Pop();

        void Apply(RectangleNode node);
        void Apply(SetColorNode node);
        void Apply(TranslateNode node);
        void Apply(RotateNode rotateNode);
        void Apply(ScaleNode scaleNode);
        void Apply(ArcNode arcNode);
        void Apply(LineNode lineNode);
    }

    public abstract class SceneNode
    {
        public List<SceneNode> Children;

        protected SceneNode()
        {
            Children = new List<SceneNode>();
        }

        public virtual void Update(ICanvas canvas)
        {
            foreach (var element in Children)
            {
                element.Update(canvas);
            }
        }
    }

    public abstract class GeometryNode : SceneNode
    {
    }

    public abstract class TransformNode : SceneNode
    {
    }

    public abstract class SetAttributeNode : SceneNode
    {
    }

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