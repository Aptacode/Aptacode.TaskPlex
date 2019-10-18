using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : IDisposable
    {
        private readonly object _mutex = new object();
        private readonly List<BaseTask> _pendingTasks;
        private readonly List<BaseTask> _runningTasks;
        private bool _isRunning;
        private readonly CancellationTokenSource _cancellationToken;

        public TaskCoordinator()
        {
            _cancellationToken = new CancellationTokenSource();
            _pendingTasks = new List<BaseTask>();
            _runningTasks = new List<BaseTask>();
            _isRunning = true;
        }

        public void Apply(BaseTask action)
        {
            lock (_mutex)
            {
                if (!_isRunning)
                {
                    return;
                }

                _pendingTasks.Add(action);
            }

            UpdateTasks();
        }

        private void UpdateTasks()
        {
            new TaskFactory().StartNew(() =>
            {
                lock (_mutex)
                {
                    if (!_isRunning)
                    {
                        return;
                    }

                    var readyTasks = GetReadyTasks();
                    if (readyTasks.Count <= 0)
                    {
                        return;
                    }

                    CleanUpPendingTasks(readyTasks);
                    StartTasks(readyTasks);
                }
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

                task.StartAsync(_cancellationToken).ContinueWith(o =>
                {
                    _runningTasks.Remove(task);
                    UpdateTasks();
                });
            }
        }
        public void Dispose()
        {
            lock (_mutex)
            {
                _isRunning = false;
                _pendingTasks.Clear();
                _runningTasks.Clear();
                _cancellationToken.Cancel();
            }
        }
    }
}