using System.Collections.Generic;
using System.Linq;
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
            _logger.LogTrace("Dispose");
            Cancel();
        }
        public void Reset()
        {
            _logger.LogTrace("Reset");
            Cancel();
            _cancellationToken = new CancellationTokenSource();
        }
        public void Cancel()
        {
            _tasks.ToList().ForEach(task => task.Cancel());
            _cancellationToken.Cancel();
        }

        public void Pause()
        {
            _logger.LogTrace("Pause");

            _tasks.ToList().ForEach(task => task.Pause());
        }

        public void Resume()
        {
            _logger.LogTrace("Resume");

            _tasks.ToList().ForEach(task => task.Resume());
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

            task.OnFinished += (s, e) => _tasks.Remove(task);
            task.OnCancelled += (s, e) => _tasks.Remove(task);

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