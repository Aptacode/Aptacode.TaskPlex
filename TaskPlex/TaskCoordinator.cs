using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : IDisposable
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly ConcurrentDictionary<int, IBaseTask> _runningTasks;
        private ConcurrentDictionary<int, ConcurrentQueue<IBaseTask>> _pendingTasks;


        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator()
        {
            _cancellationToken = new CancellationTokenSource();
            _runningTasks = new ConcurrentDictionary<int, IBaseTask>();
            _pendingTasks = new ConcurrentDictionary<int, ConcurrentQueue<IBaseTask>>();
        }

        /// <summary>
        ///     Clean up by cancelling all pending & running tasks
        /// </summary>
        public void Dispose()
        {
            _cancellationToken.Cancel();
        }

        /// <summary>
        ///     Add a task to be executed
        /// </summary>
        /// <param name="action"></param>
        public void Apply(IBaseTask task)
        {
            new TaskFactory().StartNew(() =>
            {
                if (task == null)
                {
                    return;
                }

                if (_runningTasks.TryGetValue(task.GetHashCode(), out _))
                {
                    AddToPendingTasks(task.GetHashCode(), task);
                }
                else
                {
                    AddToRunningTasks(task.GetHashCode(), task);
                }
            });
        }
 
        private void AddToRunningTasks(int hashCode, IBaseTask task)
        {
            _runningTasks.TryAdd(hashCode, task);
            StartTask(task);
        }
        private void AddToPendingTasks(int hashCode, IBaseTask task)
        {
            _pendingTasks.TryGetValue(hashCode, out var pendingTaskQueue);
            if (pendingTaskQueue == null)
            {
                var queue = new ConcurrentQueue<IBaseTask>();
                queue.Enqueue(task);
                _pendingTasks.TryAdd(hashCode, queue);
            }
            else
            {
                pendingTaskQueue.Enqueue(task);
            }
        }
        private void StartTask(IBaseTask task)
        {
            task.StartAsync(_cancellationToken).ContinueWith(o =>
            {
                RunNextTask(task.GetHashCode());

            }).ConfigureAwait(false);
        }

        private void RunNextTask(int hashCode)
        {
            if (_pendingTasks.TryGetValue(hashCode, out var taskQueue) && taskQueue.TryDequeue(out var nextTask))
            {
                AddToRunningTasks(hashCode, nextTask);
            }
            else
            {
                _pendingTasks.TryRemove(hashCode, out _);
                _runningTasks.TryRemove(hashCode, out _);
            }
        }
    }
}