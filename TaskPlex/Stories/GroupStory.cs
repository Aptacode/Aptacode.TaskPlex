using System;
using System.Collections.Generic;

namespace Aptacode.TaskPlex.Stories
{
    public abstract class GroupStory : BaseStory
    {
        protected GroupStory(TimeSpan duration, List<BaseStory> stories) : base(duration)
        {
            Stories = stories;
        }

        internal List<BaseStory> Stories { get; }

        /// <summary>
        /// Add a story to the group
        /// </summary>
        /// <param name="story"></param>
        public void Add(BaseStory story)
        {
            if (story == null)
            {
                return;
            }

            Stories.Add(story);
        }

        /// <summary>
        /// Remove a story from the group
        /// </summary>
        /// <param name="story"></param>
        public void Remove(BaseStory story)
        {
            if (story == null)
            {
                return;
            }

            Stories.Remove(story);
        }
    }
}