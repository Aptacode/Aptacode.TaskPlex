using System.Collections.Generic;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class GroupTask : BaseTask
    {
        protected GroupTask(int stepCount, List<BaseTask> tasks) : base(stepCount)
        {
            Tasks = tasks;
        }

        internal List<BaseTask> Tasks { get; }

        /// <summary>
        ///     Add a task to the group
        /// </summary>
        /// <param name="task"></param>
        public void Add(BaseTask task)
        {
            if (task == null)
            {
                return;
            }

            Tasks.Add(task);
        }

        /// <summary>
        ///     Remove a task from the group
        /// </summary>
        /// <param name="task"></param>
        public void Remove(BaseTask task)
        {
            if (task == null)
            {
                return;
            }

            Tasks.Remove(task);
        }
    }
}