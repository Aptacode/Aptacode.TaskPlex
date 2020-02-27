using System;
using System.Threading;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Tasks
{
    public abstract class BaseTask
    {
        protected BaseTask(int stepCount)
        {
            StepCount = stepCount;
            State = TaskState.Ready;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public int StepCount { get; }

        protected CancellationTokenSource CancellationTokenSource { get; private set; }

        public TaskState State { get; protected set; }

        public event EventHandler<EventArgs> OnStarted;

        public event EventHandler<EventArgs> OnFinished;

        public event EventHandler<EventArgs> OnCancelled;

        /// <summary>
        ///     Start the task with the given ParentCancellationTokenSource
        /// </summary>
        /// <returns></returns>
        public void Start(CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token);
            Setup();
            State = TaskState.Running;
            OnStarted?.Invoke(this, EventArgs.Empty);
            Begin();
        }

        protected void Finished()
        {
            State = TaskState.Stopped;
            Cleanup();

            if (!CancellationTokenSource.IsCancellationRequested)
            {
                OnFinished?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnCancelled?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Interrupt the task
        /// </summary>
        public void Cancel()
        {
            State = TaskState.Stopped;
            CancellationTokenSource.Cancel();
        }

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

        #region AbstractMethods

        protected abstract void Setup();
        protected abstract void Begin();
        public abstract void Update();
        protected abstract void Cleanup();
        public abstract void Reset();

        #endregion
    }
}