using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    ///     Manages the execution of tasks
    /// </summary>
    public class TaskCoordinator : ITaskCoordinator
    {
        private readonly ILogger _logger;
        private readonly RefreshRate _refreshRate;
        private readonly List<BaseTask> _tasks;

        private readonly Timer _taskUpdater;

        /// <summary>
        ///     Manages the execution of tasks
        /// </summary>
        public TaskCoordinator(ILoggerFactory loggerFactory, RefreshRate refreshRate)
        {
            _refreshRate = refreshRate;
            _logger = loggerFactory.CreateLogger<TaskCoordinator>();
            _logger.LogTrace("Initializing TaskCoordinator");
            _tasks = new List<BaseTask>();
            _taskUpdater = new Timer((int) _refreshRate);
            _taskUpdater.Elapsed += UpdateTasks;
        }

        public TaskState State { get; private set; }

        public IQueryable<BaseTask> GetTasks()
        {
            return _tasks.AsQueryable();
        }

        public void Stop(BaseTask task)
        {
            if (_tasks.Contains(task))
            {
                task.Cancel();
            }
        }

        public void Pause(BaseTask task)
        {
            if (_tasks.Contains(task))
            {
                task.Pause();
            }
        }

        public void Resume(BaseTask task)
        {
            if (_tasks.Contains(task))
            {
                task.Resume();
            }
        }

        /// <summary>
        ///     Cancel all tasks and create a new ParentCancellationTokenSource ready to accept new tasks
        /// </summary>
        public void Reset()
        {
            _logger.LogTrace("Reset");
            Stop();
            Start();
        }

        /// <summary>
        ///     Start
        /// </summary>
        public void Start()
        {
            State = TaskState.Running;
            _logger.LogTrace("Start");
            _taskUpdater.Start();
        }

        /// <summary>
        ///     Cancel all tasks and stop the updater
        /// </summary>
        public void Stop()
        {
            State = TaskState.Stopped;
            _logger.LogTrace("Canceled all tasks");
            _taskUpdater.Stop();
            _tasks.ForEach(task => task.Cancel());
        }

        /// <summary>
        ///     Pause all running tasks
        /// </summary>
        public void Pause()
        {
            State = TaskState.Paused;
            _logger.LogTrace("Pause");
            _tasks.ForEach(task => task.Pause());
        }

        /// <summary>
        ///     Resume all tasks
        /// </summary>
        public void Resume()
        {
            State = TaskState.Running;
            _logger.LogTrace("Resume");
            _tasks.ForEach(task => task.Resume());
        }

        /// <summary>
        ///     Start executing a given task
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
                await task.StartAsync(new CancellationTokenSource(), _refreshRate).ConfigureAwait(false);
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

        private void UpdateTasks(object sender, ElapsedEventArgs e)
        {
            _tasks.ForEach(task => task.Update());
        }
    }
}