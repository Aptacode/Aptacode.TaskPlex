using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class BaseTask
    {
        protected BaseTask(TimeSpan duration)
        {
            Duration = duration;
            CancellationToken = new CancellationTokenSource();
            State = TaskState.Ready;
        }

        public TimeSpan Duration { get; protected set; }

        protected CancellationTokenSource CancellationToken { get; private set; }

        public TaskState State { get; private set; }

        public event EventHandler<EventArgs> OnStarted;

        public event EventHandler<EventArgs> OnFinished;

        public event EventHandler<EventArgs> OnCancelled;

        /// <summary>
        /// Start the task
        /// </summary>
        /// <returns></returns>
        public Task StartAsync() => StartAsync(CancellationToken);

        /// <summary>
        /// Start the task with the given CancellationToken
        /// </summary>
        /// <returns></returns>
        public Task StartAsync(CancellationTokenSource cancellationToken)
        {
            CancellationToken = cancellationToken;
            if(!CancellationToken.IsCancellationRequested)
            {
                return InternalTask();
            }

            throw new TaskCanceledException();
        }

        /// <summary>
        /// Interrupt the task
        /// </summary>
        public void Cancel()
        {
            State = TaskState.Stopped;
            CancellationToken.Cancel();
        }

        internal void RaiseOnStarted(EventArgs args)
        {
            State = TaskState.Running;
            OnStarted?.Invoke(this, args);
        }

        internal void RaiseOnFinished(EventArgs args)
        {
            State = TaskState.Stopped;
            if(CancellationToken.IsCancellationRequested)
            {
                RaiseOnCancelled();
            } else
            {
                OnFinished?.Invoke(this, args);
            }
        }

        internal void RaiseOnCancelled()
        {
            State = TaskState.Stopped;
            OnCancelled?.Invoke(this, EventArgs.Empty);
        }

        protected abstract Task InternalTask();

        public void Pause() => State = TaskState.Paused;

        public void Resume() => State = TaskState.Running;

        protected async Task WaitUntilResumed()
        {
            while(State == TaskState.Paused)
            {
                await Task.Delay(10, CancellationToken.Token).ConfigureAwait(false);
            }
        }
    }
}