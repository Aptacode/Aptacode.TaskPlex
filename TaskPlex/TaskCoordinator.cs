using System.Collections.Generic;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator
    {
        private readonly List<BaseTask> _pendingTasks;
        private readonly List<BaseTask> _runningTasks;
        private readonly object _mutex = new object();
        public bool IsRunning { get; set; }

        public TaskCoordinator()
        {
            _pendingTasks = new List<BaseTask>();
            _runningTasks = new List<BaseTask>();
            IsRunning = false;
        }

        public void Apply(BaseTask action)
        {
            _pendingTasks.Add(action);
        }

        public void Start()
        {
            IsRunning = true;

            new TaskFactory().StartNew(() =>
            {
                Run().Wait();
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private async Task Run()
        {
            while (IsRunning)
            {
                lock (_mutex)
                {
                    List<BaseTask> readyTasks = GetReadyTasks();
                    CleanUpPendingTasks(readyTasks);
                    StartTasks(readyTasks);
                }

                await Task.Delay(1).ConfigureAwait(false);
            }
        }

        private List<BaseTask> GetReadyTasks()
        {
            List<BaseTask> readyTasks = new List<BaseTask>();

            foreach (var item in _pendingTasks)
            {
                if (!_runningTasks.Exists(t => t.CollidesWith(item)) && !readyTasks.Exists(t => t.CollidesWith(item)))
                {
                    readyTasks.Add(item);
                }
            }
            return readyTasks;
        }

        private void StartTasks(IEnumerable<BaseTask> readyTasks)
        {
            foreach (var task in readyTasks)
            {
                BaseTask localTask = task;
                _runningTasks.Add(localTask);

                localTask.OnFinished += (s, e) =>
                {
                    lock (_mutex)
                    {
                        _runningTasks.Remove(localTask);
                    }
                };

                localTask.StartAsync().ConfigureAwait(false);
            }
        }
        private void CleanUpPendingTasks(IEnumerable<BaseTask> startedTasks)
        {
            foreach (var item in startedTasks)
            {
                _pendingTasks.Remove(item);
            }
        }
    }
}
