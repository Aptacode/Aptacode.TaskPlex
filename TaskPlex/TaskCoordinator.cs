using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Aptacode.TaskPlex
{
    public class TaskCoordinator : ITaskCoordinator
    {
        private readonly ILogger _logger;

        private readonly RefreshRate _refreshRate;
        private readonly List<BaseTask> _tasks;

        private readonly Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        ///     Orchestrate the order of execution of tasks
        /// </summary>
        public TaskCoordinator(ILoggerFactory loggerFactory, RefreshRate refreshRate)
        {
            _refreshRate = refreshRate;
            _logger = loggerFactory.CreateLogger<TaskCoordinator>();
            _logger.LogTrace("Initializing TaskCoordinator");
            _cancellationTokenSource = new CancellationTokenSource();
            _tasks = new List<BaseTask>();
            _timer = new Timer((int) _refreshRate);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        /// <summary>
        ///     Clean up by canceling all pending & running tasks
        /// </summary>
        public void Dispose()
        {
            _logger.LogTrace("Dispose");
            _timer.Dispose();
            Cancel();
        }

        public void Reset()
        {
            _logger.LogTrace("Reset");
            Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Cancel()
        {
            _tasks.ForEach(task => task.Cancel());
            _cancellationTokenSource.Cancel();
        }

        public void Pause()
        {
            _logger.LogTrace("Pause");

            _tasks.ForEach(task => task.Pause());
        }

        public void Resume()
        {
            _logger.LogTrace("Resume");

            _tasks.ForEach(task => task.Resume());
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
                await task.StartAsync(_cancellationTokenSource, _refreshRate).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                _logger.LogDebug($"Task Canceled: {task}");
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _tasks.ForEach(task => task.Update());
        }
    }
}