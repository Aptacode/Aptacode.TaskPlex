using System.Collections.Generic;
using System.Linq;

namespace Aptacode.TaskPlex.Graphics.Nodes
{
    public abstract class SceneNode
    {
        public List<SceneNode> Children;

        protected SceneNode()
        {
            Children = new List<SceneNode>();
        }

        public string Name { get; set; }

        public virtual void Update(ICanvas canvas)
        {
            foreach (var element in Children)
            {
                element.Update(canvas);
            }
        }

        public IEnumerable<SceneNode> Descendants()
        {
            var nodes = new Stack<SceneNode>(new[] {this});
            while (nodes.Any())
            {
                var node = nodes.Pop();
                yield return node;
                foreach (var n in node.Children)
                {
                    nodes.Push(n);
                }
            }
        }
    }
}