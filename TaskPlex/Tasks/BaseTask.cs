using System;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class BaseTask
    {
        protected int _stepCount;

        protected BaseTask(TimeSpan duration)
        {
            Duration = duration;
            CancellationTokenSource = new CancellationTokenSource();
            State = TaskState.Ready;
        }

        public TimeSpan Duration { get; protected set; }
        protected CancellationTokenSource CancellationTokenSource { get; private set; }

        public TaskState State { get; protected set; }

        public event EventHandler<EventArgs> OnStarted;

        public event EventHandler<EventArgs> OnFinished;

        public event EventHandler<EventArgs> OnCancelled;

        /// <summary>
        ///     Start the task
        /// </summary>
        /// <returns></returns>
        public Task StartAsync()
        {
            return StartAsync(CancellationTokenSource);
        }

        /// <summary>
        ///     Start the task with the given CancellationTokenSource
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync(CancellationTokenSource cancellationTokenSource,
            RefreshRate refreshRate = RefreshRate.Normal)
        {
            CancellationTokenSource = cancellationTokenSource;
            if (!CancellationTokenSource.IsCancellationRequested)
            {
                OnStarted?.Invoke(this, EventArgs.Empty);
                _stepCount = (int) Math.Floor(Duration.TotalMilliseconds / (int) refreshRate);
                await InternalTask(refreshRate).ConfigureAwait(false);
            }
            else
            {
                State = TaskState.Stopped;
                OnCancelled?.Invoke(this, EventArgs.Empty);
                return;
            }

            State = TaskState.Stopped;
            OnFinished?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Interrupt the task
        /// </summary>
        public void Cancel()
        {
            State = TaskState.Stopped;
            CancellationTokenSource.Cancel();
        }

        protected abstract Task InternalTask(RefreshRate refreshRate);

        public virtual void Pause()
        {
            State = TaskState.Paused;
        }

        public virtual void Resume()
        {
            State = TaskState.Running;
        }

        public bool IsRunning()
        {
            return State == TaskState.Running;
        }

        protected async Task WaitUntilResumed()
        {
            while (State == TaskState.Paused)
            {
                await Task.Delay(10, CancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        public abstract void Update();
    }
}