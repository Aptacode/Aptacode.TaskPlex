using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : IDisposable
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly object _mutex = new object();
        private readonly List<IBaseTask> _pendingTasks;
        private readonly List<IBaseTask> _runningTasks;
        private bool _isRunning;

        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator()
        {
            _cancellationToken = new CancellationTokenSource();
            _pendingTasks = new List<IBaseTask>();
            _runningTasks = new List<IBaseTask>();
            _isRunning = true;
        }

        /// <summary>
        ///     Clean up by cancelling all pending & running tasks
        /// </summary>
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

        /// <summary>
        ///     Add a task to be executed
        /// </summary>
        /// <param name="action"></param>
        public void Apply(IBaseTask task)
        {
            if (task == null)
            {
                return;
            }

            lock (_mutex)
            {
                if (!_isRunning)
                {
                    return;
                }

                _pendingTasks.Add(task);
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

        private List<IBaseTask> GetReadyTasks()
        {
            var readyTasks = new List<IBaseTask>();

            foreach (var item in _pendingTasks)
            {
                if (!_runningTasks.Exists(t => t.CollidesWith(item)) && !readyTasks.Exists(t => t.CollidesWith(item)))
                {
                    readyTasks.Add(item);
                }
            }

            return readyTasks;
        }

        private void CleanUpPendingTasks(IEnumerable<IBaseTask> startedTasks)
        {
            foreach (var item in startedTasks)
            {
                _pendingTasks.Remove(item);
            }
        }

        private void StartTasks(IEnumerable<IBaseTask> readyTasks)
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
    }
}