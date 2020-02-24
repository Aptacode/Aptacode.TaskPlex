using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : ITaskCoordinator
    {
        private readonly ILogger _logger;
        private readonly HashSet<BaseTask> _tasks;

        private CancellationTokenSource _cancellationToken;

        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TaskCoordinator>();
            _logger.LogTrace("Initializing TaskCoordinator");
            _cancellationToken = new CancellationTokenSource();
            _tasks = new HashSet<BaseTask>();
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
                task.Cancel();
            }

            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        public void Pause()
        {
            foreach (var task in _tasks)
            {
                task.Pause();
            }
        }

        public void Resume()
        {
            foreach (var task in _tasks)
            {
                task.Resume();
            }
        }

        /// <summary>
        ///     Add a task to be executed
        /// </summary>
        /// <param name="task"></param>
        public async Task Apply(BaseTask task)
        {
            if (task == null)
            {
                return;
            }

            _logger.LogTrace($"Applying task: {task}");

            _tasks.Add(task);

            try
            {
                await task.StartAsync(_cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                _logger.LogDebug($"Task Canceled: {task}");
            }
        }
    }
}