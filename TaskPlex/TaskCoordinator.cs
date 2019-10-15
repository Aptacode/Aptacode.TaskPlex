using System.Collections.Generic;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator
    {
        private readonly List<BaseTask> _pendingTasks;
        private readonly List<BaseTask> _runningTasks;

        public TaskCoordinator()
        {
            _pendingTasks = new List<BaseTask>();
            _runningTasks = new List<BaseTask>();
        }

        public void Apply(BaseTask action)
        {
            _pendingTasks.Add(action);
            UpdateTasks();
        }

        private void UpdateTasks()
        {
            new TaskFactory().StartNew(() =>
            {
                var readyTasks = GetReadyTasks();
                if (readyTasks.Count <= 0) return;

                CleanUpPendingTasks(readyTasks);
                StartTasks(readyTasks);
            });
        }

        private List<BaseTask> GetReadyTasks()
        {
            var readyTasks = new List<BaseTask>();

            foreach (var item in _pendingTasks)
            {
                if (!_runningTasks.Exists(t => t.CollidesWith(item)) && !readyTasks.Exists(t => t.CollidesWith(item)))
                {
                    readyTasks.Add(item);
                }
            }
            return readyTasks;
        }
        private void CleanUpPendingTasks(IEnumerable<BaseTask> startedTasks)
        {
            foreach (var item in startedTasks)
            {
                _pendingTasks.Remove(item);
            }
        }
        private void StartTasks(IEnumerable<BaseTask> readyTasks)
        {
            foreach (var task in readyTasks)
            {
                _runningTasks.Add(task);

                task.StartAsync().ContinueWith((o) =>
                { 
                    _runningTasks.Remove(task);
                    UpdateTasks();
                });
            }
        }
    }
}
