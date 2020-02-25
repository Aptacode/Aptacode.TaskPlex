using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Aptacode.TaskPlex
{
    /// <summary>
    /// Manages the execution of tasks
    /// </summary>
    public class TaskCoordinator : ITaskCoordinator
    {
        private readonly ILogger _logger;
        private readonly RefreshRate _refreshRate;

        private CancellationTokenSource _cancellationTokenSource;
        private readonly Timer _taskUpdater;
        private readonly List<BaseTask> _tasks;
        public TaskState State { get; private set; } 

        /// <summary>
        /// Manages the execution of tasks
        /// </summary>
        public TaskCoordinator(ILoggerFactory loggerFactory, RefreshRate refreshRate)
        {
            _refreshRate = refreshRate;
            _logger = loggerFactory.CreateLogger<TaskCoordinator>();
            _logger.LogTrace("Initializing TaskCoordinator");
            _cancellationTokenSource = new CancellationTokenSource();
            _tasks = new List<BaseTask>();
            _taskUpdater = new Timer((int) _refreshRate);
            _taskUpdater.Elapsed += (s,e) => _tasks.ForEach(task => task.Update());
            _taskUpdater.Start();
            State = TaskState.Running;
        }

        /// <summary>
        /// Stop all tasks and release all resources
        /// </summary>
        public void Dispose()
        {
            State = TaskState.Stopped;
            _logger.LogTrace("Dispose");
            _taskUpdater.Stop();
            _taskUpdater.Dispose();
            CancelAll();
        }

        /// <summary>
        /// Cancel all tasks and create a new CancellationTokenSource ready to accept new tasks
        /// </summary>
        public void Reset()
        {
            State = TaskState.Running;
            _logger.LogTrace("Reset");
            CancelAll();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Cancel all tasks
        /// </summary>
        public void CancelAll()
        {
            State = TaskState.Stopped;
            _logger.LogTrace("Canceled all tasks");
            _tasks.ForEach(task => task.Cancel());
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Pause all running tasks
        /// </summary>
        public void Pause()
        {
            State = TaskState.Paused;
            _logger.LogTrace("Pause");
            _tasks.ForEach(task => task.Pause());
        }
        /// <summary>
        /// Resume all tasks
        /// </summary>
        public void Resume()
        {
            State = TaskState.Running;
            _logger.LogTrace("Resume");
            _tasks.ForEach(task => task.Resume());
        }

        /// <summary>
        /// Start executing a given task
        /// </summary>
        /// <param name="task"></param>
        public async Task Apply(BaseTask task)
        {
            //Return null if the tasks given was null
            if (task == null)
            {
                return;
            }

            _logger.LogTrace($"Applying task: {task}");
            //Add the task to the list for updating
            _tasks.Add(task);

            try
            {
                //Run the task asynchronously with the Coordinators cancellation token source and refresh rate
                await task.StartAsync(_cancellationTokenSource, _refreshRate).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Task Canceled: {task}");
            }
            finally
            {
                //When the task is finished remove it from the list
                _tasks.Remove(task);
            }
        }
    }
}