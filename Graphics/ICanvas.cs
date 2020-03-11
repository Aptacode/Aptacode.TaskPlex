using Aptacode.TaskPlex.Graphics.Nodes.Attribute;
using Aptacode.TaskPlex.Graphics.Nodes.Geometry;
using Aptacode.TaskPlex.Graphics.Nodes.Transform;

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
        void Setup();
        void Update(Scene scene);
    }
}