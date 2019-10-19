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

            TryToStartTask(task);
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
                StartTask(task).ConfigureAwait(false);
            }
        }

        private async Task StartTask(BaseTask task)
        {
            task.RaiseOnStarted(EventArgs.Empty);
            try
            {
                if (task is ParallelGroupTask parallelGroupTask)
                {
                    await RunParallel(parallelGroupTask).ConfigureAwait(false);
                }
                else if (task is SequentialGroupTask sequentialGroupTask)
                {
                    await RunSequential(sequentialGroupTask).ConfigureAwait(false);
                }
                else
                {
                    await Run(task).ConfigureAwait(false);
                }

                task.RaiseOnFinished(EventArgs.Empty);
                RunNextTask(task);
            }
            catch (TaskCanceledException)
            {
                task.RaiseOnCancelled();
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

            bool isRunning = true;

            int finishedTaskCount = 0;
            foreach (var taskTask in task.Tasks)
            {
                taskTask.OnFinished += (s, e) => {
                    if (++finishedTaskCount >= task.Tasks.Count)
                    {
                        isRunning = false;
                    }
                };
                Apply(taskTask);
            }

            while (isRunning)
            {
                await Task.Delay(1).ConfigureAwait(false);
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
            //When the last task finishes set running to false
            task.Tasks[task.Tasks.Count - 1].OnFinished += (s, e) => { isRunning = false; };

            Apply(task.Tasks[0]);

            while (isRunning)
            {
                await Task.Delay(1).ConfigureAwait(false);
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
