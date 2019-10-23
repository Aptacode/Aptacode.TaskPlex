using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using NLog;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private CancellationTokenSource _cancellationToken;
        private readonly ConcurrentDictionary<BaseTask, ConcurrentQueue<BaseTask>> _tasks;

        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator()
        {
            Logger.Trace("Initialising TaskCoordinator");
            _cancellationToken = new CancellationTokenSource();
            _tasks = new ConcurrentDictionary<BaseTask, ConcurrentQueue<BaseTask>>();
        }

        /// <summary>
        ///     Clean up by cancelling all pending & running tasks
        /// </summary>
        public void Dispose()
        {
            Logger.Trace("Disposing");
            _cancellationToken.Cancel();
        }        
        
        public void Reset()
        {
            Logger.Trace("Reseting");
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
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

            Logger.Trace($@"Applying task: {task.ToString()}");
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

                Logger.Trace($@"Queued task: {task.ToString()}");
                taskQueue?.Enqueue(task);
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
                Logger.Trace($@"Task Started: {task.ToString()}");
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

                Logger.Trace($@"Task Finished: {task.ToString()}");
                task.RaiseOnFinished(EventArgs.Empty);
                RunNextTask(task);
            }
            catch (TaskCanceledException)
            {
                Logger.Trace($@"Task Cancelled: {task.ToString()}");
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

            var isRunning = true;
            var finishedTaskCount = 0;

            foreach (var taskTask in task.Tasks)
            {
                taskTask.OnFinished += (s, e) => {
                    if (++finishedTaskCount >= task.Tasks.Count)
                    {
                        isRunning = false;
                    }
                };
                taskTask.OnCancelled += (s, e) =>
                {
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
                tasks[localIndex - 1].OnCancelled += (s, e) => { Apply(tasks[localIndex]); };
            }
        }

        private void RunNextTask(BaseTask completedTask)
        {
            if (_tasks.TryGetValue(completedTask, out var taskQueue) && taskQueue.TryDequeue(out var nextTask))
            {
                StartTask(nextTask).ConfigureAwait(false);
            }
            else
            {
                _tasks.TryRemove(completedTask, out _);
            }
        }
    }
}
