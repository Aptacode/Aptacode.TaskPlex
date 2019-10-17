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

        public void Add(BaseTask task)
        {
            Tasks.Add(task);
            Duration = GetTotalDuration(Tasks);
        }

        public void Remove(BaseTask task)
        {
            Tasks.Remove(task);
            Duration = GetTotalDuration(Tasks);
        }

        protected abstract TimeSpan GetTotalDuration(IEnumerable<BaseTask> tasks);
    }
}