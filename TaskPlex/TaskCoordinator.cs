using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : ITaskCoordinator
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

            foreach (var task in _tasks)
            {
                task.Key.Cancel();
            }

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
            if (!_tasks.ContainsKey(task))
            {
                return true;
            }

            _tasks.TryGetValue(task, out var taskQueue);
            if (taskQueue == null)
            {
                taskQueue = new ConcurrentQueue<BaseTask>();
                _tasks.AddOrUpdate(task, taskQueue, (s, q) => taskQueue);
            }

            _logger.LogTrace($"Queued task: {task}");
            taskQueue.Enqueue(task);


            return false;
        }

        private async Task StartTask(BaseTask task)
        {
            _tasks.TryAdd(task, null);

            try
            {
                _logger.LogTrace($"Task Started: {task}");

                if (task is GroupTask groupTask)
                {
                    groupTask.SetTaskCoordinator(this);
                }

                await task.StartAsync(_cancellationToken).ConfigureAwait(false);

                _logger.LogTrace($"Task Finished: {task}");
            }
            catch (TaskCanceledException)
            {
                _logger.LogDebug($"Task Canceled: {task}");
            }
            finally
            {
                RunNextTask(task);
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