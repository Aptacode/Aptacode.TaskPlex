using System.Drawing;
using System.Linq;
using System.Numerics;
using Aptacode.TaskPlex.Graphics.Nodes;
using Aptacode.TaskPlex.Graphics.Nodes.Attribute;
using Aptacode.TaskPlex.Graphics.Nodes.Geometry;

namespace Aptacode.TaskPlex.Graphics
{
    public class Scene
    {
        private readonly SceneNode _rootNode;

        public Scene()
        {
            var clearBackground = new RectangleNode {Position = new Vector2(0, 0), Size = new Vector2(1000, 1000)};
            var setbackground = new SetColorNode {Color = Color.White};
            setbackground.Children.Add(clearBackground);

            _rootNode = setbackground;
        }

        public SceneNode this[string nodeName]
        {
            get { return _rootNode.Descendants().Where(node => node.Name == nodeName).FirstOrDefault(); }
        }

        public void Update(ICanvas canvas)
        {
            _rootNode.Update(canvas);
        }

        public void Add(SceneNode node)
        {
            _rootNode.Children.Add(node);
        }
    }
}