using System.Collections.Generic;

namespace Aptacode.TaskPlex.Graphics.Nodes
{
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
}