using System;
using System.Collections.Generic;

namespace Aptacode.Core.Tasks
{
    public abstract class GroupTask : BaseTask
    {
        protected List<BaseTask> Tasks { get; set; }

        protected GroupTask(IEnumerable<BaseTask> tasks)
        {
            Tasks = new List<BaseTask>(tasks);
        }

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
