using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.EventArgs;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class BaseTask : IBaseTask
    {
        protected BaseTask(TimeSpan duration)
        {
            Duration = duration;
            _cancellationToken = new CancellationTokenSource();
        }

        protected BaseTask() : this(TimeSpan.Zero)
        {
        }

        public TimeSpan Duration { get; set; }
        protected CancellationTokenSource _cancellationToken { get; set; }

        public event EventHandler<System.EventArgs> OnStarted;
        public event EventHandler<System.EventArgs> OnFinished;
        public event EventHandler<TaskCancellationEventArgs> OnCancelled;
        public abstract bool CollidesWith(IBaseTask item);
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

        protected abstract Task InternalTask();

        protected void RaiseOnStarted(System.EventArgs args)
        {
            OnStarted?.Invoke(this, args);
        }

        protected void RaiseOnFinished(System.EventArgs args)
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

        protected void RaiseOnCancelled()
        {
            OnCancelled?.Invoke(this, new TaskCancellationEventArgs());
        }


    }
}