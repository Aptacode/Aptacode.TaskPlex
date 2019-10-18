using System;
using System.Collections.Generic;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class GroupTask : BaseTask
    {
        protected GroupTask(IEnumerable<BaseTask> tasks)
        {
            Tasks = new List<BaseTask>(tasks);
        }

        protected List<BaseTask> Tasks { get; set; }

        /// <summary>
        ///     Add a task to the group
        /// </summary>
        /// <param name="task"></param>
        public void Add(BaseTask task)
        {
            Tasks.Add(task);
            Duration = GetTotalDuration(Tasks);
        }

        /// <summary>
        ///     Remove a task from the group
        /// </summary>
        /// <param name="task"></param>
        public void Remove(BaseTask task)
        {
            Tasks.Remove(task);
            Duration = GetTotalDuration(Tasks);
        }

        protected abstract TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks);
    }
}