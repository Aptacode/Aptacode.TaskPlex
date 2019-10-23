using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class BaseTask
    {
        protected BaseTask(TimeSpan duration)
        {
            Duration = duration;
            CancellationToken = new CancellationTokenSource();
        }

        public TimeSpan Duration { get; set; }
        protected CancellationTokenSource CancellationToken { get; set; }

        public event EventHandler<EventArgs> OnStarted;
        public event EventHandler<EventArgs> OnFinished;
        public event EventHandler<EventArgs> OnCancelled;

        /// <summary>
        ///     Start the task
        /// </summary>
        /// <returns></returns>
        public Task StartAsync()
        {
            return StartAsync(CancellationToken);
        }

        /// <summary>
        ///     Start the task with the given CancellationToken
        /// </summary>
        /// <returns></returns>
        public Task StartAsync(CancellationTokenSource cancellationToken)
        {
            CancellationToken = cancellationToken;
            if (!CancellationToken.IsCancellationRequested)
            {
                return InternalTask();
            }

            RaiseOnCancelled();
            return Task.FromCanceled(CancellationToken.Token);
        }

        /// <summary>
        ///     Interrupt the task
        /// </summary>
        public void Cancel()
        {
            CancellationToken.Cancel();
        }

        internal void RaiseOnStarted(EventArgs args)
        {
            OnStarted?.Invoke(this, args);
        }

        internal void RaiseOnFinished(EventArgs args)
        {
            if (CancellationToken.IsCancellationRequested)
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