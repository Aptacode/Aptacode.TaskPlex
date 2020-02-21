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
            return StartAsync(CancellationToken);
        }

        /// <summary>
        ///     Start the task with the given CancellationToken
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync(CancellationTokenSource cancellationToken)
        {
            CancellationToken = cancellationToken;
            if (!CancellationToken.IsCancellationRequested)
            {
                await InternalTask().ConfigureAwait(false);
            }
            else
            {
                throw new TaskCanceledException();
            }
        }

        /// <summary>
        ///     Interrupt the task
        /// </summary>
        public void Cancel()
        {
            State = TaskState.Stopped;
            CancellationToken.Cancel();
        }

        internal async Task RaiseOnStarted(EventArgs args)
        {
            await new TaskFactory(CancellationToken.Token).StartNew(() => OnStarted?.Invoke(this, args))
                .ConfigureAwait(false);
        }

        internal async Task RaiseOnFinished(EventArgs args)
        {
            if (CancellationToken.IsCancellationRequested)
            {
                await RaiseOnCancelled().ConfigureAwait(false);
            }
            else
            {
                _ = new TaskFactory(CancellationToken.Token).StartNew(() => { OnFinished?.Invoke(this, args); })
                    .ConfigureAwait(false);
            }
        }

        internal async Task RaiseOnCancelled()
        {
            State = TaskState.Stopped;
            await new TaskFactory(CancellationToken.Token).StartNew(() =>
            {
                OnCancelled?.Invoke(this, EventArgs.Empty);
            }).ConfigureAwait(false);
        }

        protected abstract Task InternalTask();

        public void Pause()
        {
            State = TaskState.Paused;
        }

        public void Resume()
        {
            State = TaskState.Running;
        }

        protected async Task WaitUntilResumed()
        {
            while (State == TaskState.Paused)
            {
                await Task.Delay(10, CancellationToken.Token).ConfigureAwait(false);
            }
        }

        public abstract override bool Equals(object obj);
    }
}