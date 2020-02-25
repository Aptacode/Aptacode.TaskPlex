using System;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class BaseTask : IDisposable
    {
        protected BaseTask(TimeSpan duration)
        {
            Duration = duration;
            CancellationTokenSource = new CancellationTokenSource();
            State = TaskState.Ready;
        }

        public TimeSpan Duration { get; protected set; }
        protected CancellationTokenSource CancellationTokenSource { get; private set; }
        public TaskState State { get; protected set; }
        public abstract void Dispose();

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
        public async Task StartAsync(CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = cancellationTokenSource;
            if (!CancellationTokenSource.IsCancellationRequested)
            {
                Setup();
                State = TaskState.Running;

                OnStarted?.Invoke(this, EventArgs.Empty);

                await InternalTask().ConfigureAwait(false);
            }
            else
            {
                State = TaskState.Stopped;
                OnCancelled?.Invoke(this, EventArgs.Empty);
                Dispose();
                return;
            }

            State = TaskState.Stopped;
            OnFinished?.Invoke(this, EventArgs.Empty);

            Dispose();
        }

        protected abstract void Setup();

        /// <summary>
        ///     Interrupt the task
        /// </summary>
        public void Cancel()
        {
            State = TaskState.Stopped;
            CancellationTokenSource.Cancel();
        }

        protected abstract Task InternalTask();

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
    }
}