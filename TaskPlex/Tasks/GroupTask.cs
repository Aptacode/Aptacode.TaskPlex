using System;
using System.Collections.Generic;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class GroupTask : BaseTask
    {
        protected GroupTask(IEnumerable<IBaseTask> tasks)
        {
            Tasks = new List<IBaseTask>(tasks);
        }

        protected List<IBaseTask> Tasks { get; set; }

        /// <summary>
        ///     Add a task to the group
        /// </summary>
        /// <param name="task"></param>
        public void Add(IBaseTask task)
        {
            if(task == null)
            {
                return;
            }

            Tasks.Add(task);
            Duration = GetTotalDuration(Tasks);
        }

        /// <summary>
        ///     Remove a task from the group
        /// </summary>
        /// <param name="task"></param>
        public void Remove(IBaseTask task)
        {
            if (task == null)
            {
                return;
            }

            Tasks.Remove(task);
            Duration = GetTotalDuration(Tasks);
        }

        protected abstract TimeSpan GetTotalDuration(IEnumerable<IBaseTask> tasks);
    }
}