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
        private readonly ConcurrentDictionary<BaseTask, ConcurrentQueue<BaseTask>> _tasks;

        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator()
        {
            _cancellationToken = new CancellationTokenSource();
            _tasks = new ConcurrentDictionary<BaseTask, ConcurrentQueue<BaseTask>>();
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
                TryToStartTask(task);
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

                    RunNextTask(task);
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
                        Task.Delay(1).Wait();
                    }

                }, _cancellationToken.Token).ContinueWith(o =>
                {
                    task.RaiseOnFinished(EventArgs.Empty);

                    RunNextTask(task);
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

        private void TryToStartTask(BaseTask task)
        {
            if (_tasks.TryGetValue(task, out var taskQueue))
            {
                if (taskQueue == null)
                {
                    taskQueue = new ConcurrentQueue<BaseTask>();
                    _tasks.TryAdd(task, taskQueue);
                }

                taskQueue.Enqueue(task);
            }
            else
            {
                StartTask(task);
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

                    RunNextTask(task);

                }).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                task.RaiseOnCancelled();
            }
        }

        private void RunNextTask(BaseTask completedTask)
        {
            if (_tasks.TryGetValue(completedTask, out var taskQueue) && taskQueue.TryDequeue(out var nextTask))
            {
                StartTask(nextTask);
            }
            else
            {
                _tasks.TryRemove(completedTask, out _);
            }
        }
    }
}
