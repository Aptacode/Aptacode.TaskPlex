using System.Drawing;
using System.Linq;
using Aptacode.TaskPlex.Graphics.Nodes;
using Aptacode.TaskPlex.Graphics.Nodes.Attribute;

namespace Aptacode.TaskPlex.Graphics
{
    public class Scene
    {
        public Scene()
        {
            Root = new SetColorNode {Color = Color.White};
        }

        public SceneNode Root { get; set; }

        public SceneNode this[string nodeName]
        {
            get { return Root.Descendants().FirstOrDefault(node => node.Name == nodeName); }
        }

        public SceneNode Parent(SceneNode node)
        {
            return node == Root ? Root : Root.Descendants().FirstOrDefault(n => n.Children.Contains(node));
        }
    }
}