using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : IDisposable
    {
        private readonly CancellationTokenSource _cancellationToken;
        private readonly ConcurrentDictionary<int, ConcurrentQueue<BaseTask>> _pendingTasks;
        private readonly ConcurrentDictionary<int, BaseTask> _runningTasks;

        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator()
        {
            _cancellationToken = new CancellationTokenSource();
            _runningTasks = new ConcurrentDictionary<int, BaseTask>();
            _pendingTasks = new ConcurrentDictionary<int, ConcurrentQueue<BaseTask>>();
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
        public void Apply(BaseTask task)
        {
            if (task == null)
            {
                return;
            }

            if (task is ParallelGroupTask parallelGroupTask)
            {
                ApplyParallel(parallelGroupTask);
            }
            else if (task is SequentialGroupTask sequentialGroupTask)
            {
                ApplySequential(sequentialGroupTask);
            }
            else
            {
                if (_runningTasks.TryGetValue(task.GetHashCode(), out _))
                {
                    AddToPendingTasks(task.GetHashCode(), task);
                }
                else
                {
                    AddToRunningTasks(task.GetHashCode(), task);
                }
            }
        }

        private void ApplyParallel(ParallelGroupTask task)
        {
            task.RaiseOnStarted(EventArgs.Empty);

            try
            {
                new TaskFactory().StartNew(() =>
                {
                    foreach (var taskTask in task.Tasks)
                    {
                        Apply(taskTask);
                    }
                }, _cancellationToken.Token).ContinueWith(o =>
                {
                    task.RaiseOnFinished(EventArgs.Empty);

                    RunNextTask(task.GetHashCode());
                }).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                task.RaiseOnCancelled();
            }
        }

        private void ApplySequential(SequentialGroupTask task)
        {
            task.RaiseOnStarted(EventArgs.Empty);
            try
            {
                new TaskFactory().StartNew(() =>
                {
                    if (task.Tasks.Count == 0)
                    {
                        return;
                    }

                    ConnectSequentialTasks(task.Tasks);

                    var running = true;
                    task.Tasks[task.Tasks.Count - 1].OnFinished += (s, e) => { running = false; };

                    Apply(task.Tasks[0]);

                    while (running)
                    {
                        Task.Delay(1).ConfigureAwait(false);
                    }
                }, _cancellationToken.Token).ContinueWith(o =>
                {
                    task.RaiseOnFinished(EventArgs.Empty);

                    RunNextTask(task.GetHashCode());
                }).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                task.RaiseOnCancelled();
            }
        }

        private void ConnectSequentialTasks(List<BaseTask> tasks)
        {
            for (var i = 1; i < tasks.Count; i++)
            {
                var localIndex = i;
                tasks[localIndex - 1].OnFinished += (s, e) => { Apply(tasks[localIndex]); };
            }
        }

        private void AddToRunningTasks(int hashCode, BaseTask task)
        {
            _runningTasks.TryAdd(hashCode, task);
            StartTask(task);
        }

        private void AddToPendingTasks(int hashCode, BaseTask task)
        {
            _pendingTasks.TryGetValue(hashCode, out var pendingTaskQueue);
            if (pendingTaskQueue == null)
            {
                var queue = new ConcurrentQueue<BaseTask>();
                queue.Enqueue(task);
                _pendingTasks.TryAdd(hashCode, queue);
            }
            else
            {
                pendingTaskQueue.Enqueue(task);
            }
        }

        private void StartTask(BaseTask task)
        {
            task.RaiseOnStarted(EventArgs.Empty);

            try
            {
                task.StartAsync(_cancellationToken).ContinueWith(o =>
                {
                    task.RaiseOnFinished(EventArgs.Empty);

                    RunNextTask(task.GetHashCode());
                }).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                task.RaiseOnCancelled();
            }
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
