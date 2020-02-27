using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging;

namespace Aptacode.TaskPlex
{
    /// <summary>
    ///     Manages the execution of tasks
    /// </summary>
    public class TaskCoordinator : ITaskCoordinator
    {
        private static readonly object ListLock = new object();
        private readonly ILogger _logger;
        private readonly List<BaseTask> _tasks;

        private readonly IUpdater _taskUpdater;

        /// <summary>
        ///     Manages the execution of tasks
        /// </summary>
        public TaskCoordinator(ILoggerFactory loggerFactory, IUpdater taskUpdater)
        {
            _taskUpdater = taskUpdater;
            _taskUpdater.OnUpdate += UpdateTasks;
            _logger = loggerFactory.CreateLogger<TaskCoordinator>();
            _logger.LogTrace("Initializing TaskCoordinator");
            _tasks = new List<BaseTask>();
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
            _tasks.ToList().ForEach(task => task.Cancel());
        }

        /// <summary>
        ///     Pause all running tasks
        /// </summary>
        public void Pause()
        {
            State = TaskState.Paused;
            _logger.LogTrace("Pause");
            _tasks.ToList().ForEach(task => task.Pause());
        }

        /// <summary>
        ///     Resume all tasks
        /// </summary>
        public void Resume()
        {
            State = TaskState.Running;
            _logger.LogTrace("Resume");
            _tasks.ToList().ForEach(task => task.Resume());
        }

        /// <summary>
        ///     Start executing a given task
        /// </summary>
        /// <param name="task"></param>
        public void Apply(BaseTask task)
        {
            //Return null if the tasks given was null
            if (task == null)
            {
                return;
            }

            _logger.LogTrace($"Applying task: {task}");
            if (_tasks.Contains(task))
            {
                task.Reset();
            }
            else
            {
                //Add the task to the list for updating
                lock (ListLock)
                {
                    _tasks.Add(task);
                }

                //When the task is finished remove it from the list
                task.OnFinished += Task_OnFinished;
                task.OnCancelled += Task_OnFinished;
            }

            try
            {
                //Run the task asynchronously with the Coordinators cancellation token source and refresh rate
                task.Start(new CancellationTokenSource());
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Task Failed: {task}");
            }
        }

        private void Task_OnFinished(object sender, EventArgs e)
        {
            if (!(sender is BaseTask task))
            {
                return;
            }

            lock (ListLock)
            {
                _tasks.Remove(task);
            }

            task.OnFinished -= Task_OnFinished;
            task.OnCancelled -= Task_OnFinished;
        }

        private void UpdateTasks(object sender, EventArgs e)
        {
            if (State != TaskState.Running)
            {
                return;
            }

            lock (ListLock)
            {
                _tasks.ForEach(task => Task.Run(task.Update));
            }
        }
    }
}