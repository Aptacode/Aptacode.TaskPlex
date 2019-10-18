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
            _cancellationToken = new CancellationTokenSource();
        }

        public TimeSpan Duration { get; set; }
        protected CancellationTokenSource _cancellationToken { get; set; }

        public event EventHandler<EventArgs> OnStarted;
        public event EventHandler<EventArgs> OnFinished;
        public event EventHandler<EventArgs> OnCancelled;

        /// <summary>
        ///     Start the task
        /// </summary>
        /// <returns></returns>
        public Task StartAsync()
        {
            return StartAsync(_cancellationToken);
        }

        /// <summary>
        ///     Start the task with the given CancellationToken
        /// </summary>
        /// <returns></returns>
        public Task StartAsync(CancellationTokenSource cancellationToken)
        {
            _cancellationToken = cancellationToken;
            if (_cancellationToken.IsCancellationRequested)
            {
                RaiseOnCancelled();
                return Task.FromCanceled(_cancellationToken.Token);
            }

            return InternalTask();
        }

        /// <summary>
        ///     Interrupt the task
        /// </summary>
        public void Cancel()
        {
            _cancellationToken.Cancel();
        }

        internal void RaiseOnStarted(EventArgs args)
        {
            OnStarted?.Invoke(this, args);
        }

        internal void RaiseOnFinished(EventArgs args)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                RaiseOnCancelled();
            }
            else
            {
                OnFinished?.Invoke(this, args);
            }
        }

        internal void RaiseOnCancelled()
        {
            OnCancelled?.Invoke(this, EventArgs.Empty);
        }

        protected abstract Task InternalTask();
    }
}