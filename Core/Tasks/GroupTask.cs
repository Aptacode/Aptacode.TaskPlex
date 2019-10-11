using System;
using System.Collections.Generic;

namespace Aptacode.Core.Tasks
{
    public abstract class GroupTask : BaseTask
    {
        protected List<BaseTask> Tasks { get; set; }

        public GroupTask(IEnumerable<BaseTask> tasks) : base()
        {
            Tasks = new List<BaseTask>(tasks);
            Duration = GetTotalDuration(Tasks);
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
