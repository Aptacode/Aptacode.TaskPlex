using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class BaseTaskEventArgs : EventArgs
    {
    }

    public class TaskCancellationEventArgs : BaseTaskEventArgs
    {
    }

    public abstract class BaseTask
    {

        public event EventHandler<BaseTaskEventArgs> OnStarted;
        public event EventHandler<BaseTaskEventArgs> OnFinished;
        public event EventHandler<TaskCancellationEventArgs> OnCancelled;
        public TimeSpan Duration { get; set; }
        protected CancellationTokenSource _cancellationToken { get; set; }
        protected BaseTask(TimeSpan duration)
        {
            Duration = duration;
            _cancellationToken = new CancellationTokenSource();
        }

        protected BaseTask() : this(TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Returns true if the specified task collides with the instance
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract bool CollidesWith(BaseTask item);

        /// <summary>
        /// Start the task
        /// </summary>
        /// <returns></returns>
        public Task StartAsync()
        {
            return StartAsync(_cancellationToken);
        }
        /// <summary>
        /// Start the task with the given CancellationToken
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
        /// Interrupt the task
        /// </summary>
        public void Cancel()
        {
            _cancellationToken.Cancel();
        }

        protected abstract Task InternalTask();

        protected void RaiseOnStarted(BaseTaskEventArgs args)
        {
            OnStarted?.Invoke(this, args);
        }

        protected void RaiseOnFinished(BaseTaskEventArgs args)
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