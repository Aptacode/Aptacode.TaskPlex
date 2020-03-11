using System;
using System.Collections.Generic;

namespace Aptacode.TaskPlex.Stories
{
    public abstract class GroupStory : BaseStory
    {
        protected GroupStory(TimeSpan duration, List<BaseStory> tasks) : base(duration)
        {
            Tasks = tasks;
        }

        internal List<BaseStory> Tasks { get; }

        /// <summary>
        ///     Add a task to the group
        /// </summary>
        /// <param name="story"></param>
        public void Add(BaseStory story)
        {
            if (story == null)
            {
                return;
            }

            Tasks.Add(story);
        }

        /// <summary>
        ///     Remove a task from the group
        /// </summary>
        /// <param name="story"></param>
        public void Remove(BaseStory story)
        {
            if (story == null)
            {
                return;
            }

            Tasks.Remove(story);
        }
    }
}