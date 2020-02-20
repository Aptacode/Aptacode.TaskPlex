using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : IDisposable
    {
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<BaseTask, ConcurrentQueue<BaseTask>> _tasks;

        private CancellationTokenSource _cancellationToken;

        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TaskCoordinator>();

            _logger.LogTrace("Initializing TaskCoordinator");
            _cancellationToken = new CancellationTokenSource();
            _tasks = new ConcurrentDictionary<BaseTask, ConcurrentQueue<BaseTask>>();
        }

        /// <summary>
        ///     Clean up by canceling all pending & running tasks
        /// </summary>
        public void Dispose()
        {
            _logger.LogTrace("Disposing");
            _cancellationToken.Cancel();
        }

        public void Reset()
        {
            _logger.LogTrace("Resetting");
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        public void Pause()
        {
            foreach (var task in _tasks)
            {
                task.Key.Pause();
            }
        }

        public void Resume()
        {
            foreach (var task in _tasks)
            {
                task.Key.Resume();
            }
        }

        /// <summary>
        ///     Add a task to be executed
        /// </summary>
        /// <param name="task"></param>
        public void Apply(BaseTask task)
        {
            if (task == null)
            {
                return;
            }

            _logger.LogTrace($"Applying task: {task}");
            TryRunTask(task);
        }

        private void TryRunTask(BaseTask task)
        {
            if (CanRunTask(task))
            {
                StartTask(task).ConfigureAwait(false);
            }
        }

        private bool CanRunTask(BaseTask task)
        {
            if (!_tasks.TryGetValue(task, out var taskQueue))
            {
                return true;
            }

            if (taskQueue == null)
            {
                taskQueue = new ConcurrentQueue<BaseTask>();
                _tasks.TryAdd(task, taskQueue);
            }

            _logger.LogTrace($"Queued task: {task}");
            taskQueue.Enqueue(task);
            return false;
        }

        private async Task StartTask(BaseTask task)
        {
            _tasks.TryAdd(task, null);

            task.RaiseOnStarted(EventArgs.Empty);
            try
            {
                _logger.LogTrace($"Task Started: {task}");
                switch (task)
                {
                    case ParallelGroupTask parallelGroupTask:
                        await RunParallel(parallelGroupTask).ConfigureAwait(false);
                        break;
                    case SequentialGroupTask sequentialGroupTask:
                        await RunSequential(sequentialGroupTask).ConfigureAwait(false);
                        break;
                    default:
                        await Run(task).ConfigureAwait(false);
                        break;
                }

                _logger.LogTrace($"Task Finished: {task}");
                task.RaiseOnFinished(EventArgs.Empty);
            }
            catch (TaskCanceledException)
            {
                _logger.LogDebug($"Task Cancelled: {task}");
                task.RaiseOnCancelled();
            }
            finally
            {
                RunNextTask(task);
            }
        }

        private async Task Run(BaseTask task)
        {
            await task.StartAsync(_cancellationToken).ConfigureAwait(false);
        }

        private async Task RunParallel(ParallelGroupTask task)
        {
            if (task.Tasks.Count == 0)
            {
                return;
            }

            var isRunning = true;
            var finishedTaskCount = 0;

            task.OnCancelled += (s, e) =>
            {
                foreach (var task1 in task.Tasks)
                {
                    task1.Cancel();
                }

                isRunning = false;
            };

            var runnableTasks = new List<Task>();

            foreach (var childTask in task.Tasks)
            {
                childTask.OnFinished += (s, e) =>
                {
                    if (++finishedTaskCount >= task.Tasks.Count)
                    {
                        isRunning = false;
                    }
                };
                childTask.OnCancelled += (s, e) =>
                {
                    if (++finishedTaskCount >= task.Tasks.Count)
                    {
                        isRunning = false;
                    }
                };

                if (CanRunTask(childTask))
                {
                    runnableTasks.Add(StartTask(childTask));
                }
            }

            await Task.WhenAll(runnableTasks).ConfigureAwait(false);


            while (isRunning)
            {
                await Task.Delay(15).ConfigureAwait(false);
            }
        }


        private async Task RunSequential(SequentialGroupTask task)
        {
            if (task.Tasks.Count == 0)
            {
                return;
            }

            ConnectSequentialTasks(task.Tasks);

            var isRunning = true;

            task.OnCancelled += (s, e) =>
            {
                foreach (var task1 in task.Tasks)
                {
                    task1.Cancel();
                }

                isRunning = false;
            };

            //When the last task finishes set running to false
            task.Tasks[task.Tasks.Count - 1].OnFinished += (s, e) => isRunning = false;

            Apply(task.Tasks[0]);

            while (isRunning)
            {
                await Task.Delay(15).ConfigureAwait(false);
            }
        }

        private void ConnectSequentialTasks(List<BaseTask> tasks)
        {
            for (var i = 1; i < tasks.Count; i++)
            {
                var localIndex = i;
                tasks[localIndex - 1].OnFinished += (s, e) => Apply(tasks[localIndex]);
            }
        }

        private void RunNextTask(BaseTask completedTask)
        {
            if (_tasks.TryGetValue(completedTask, out var taskQueue) && taskQueue != null)
            {
                if (taskQueue.TryDequeue(out var nextTask))
                {
                    StartTask(nextTask).ConfigureAwait(false);
                }
            }
            else
            {
                _tasks.TryRemove(completedTask, out _);
            }
        }
    }
}