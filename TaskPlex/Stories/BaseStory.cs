using System;
using System.Threading;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Stories
{
    public abstract class BaseStory
    {
        protected BaseStory(TimeSpan duration)
        {
            Duration = duration;
            State = StoryState.Ready;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public RefreshRate RefreshRate { get; set; }
        public TimeSpan Duration { get; set; }
        public int StepCount { get; private set; }
        protected CancellationTokenSource CancellationTokenSource { get; private set; }
        public StoryState State { get; protected set; }
        public bool IsCancelled => CancellationTokenSource.IsCancellationRequested;

        public event EventHandler<EventArgs> OnStarted;

        public event EventHandler<EventArgs> OnFinished;

        public event EventHandler<EventArgs> OnCancelled;

        /// <summary>
        /// Setup and run the story using the parents cancellationTokenSource & refreshRate
        /// </summary>
        /// <returns></returns>
        public void Start(CancellationTokenSource cancellationTokenSource, RefreshRate refreshRate)
        {
            RefreshRate = refreshRate;
            StepCount = (int) Duration.TotalMilliseconds / (int) RefreshRate;
            CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token);
            Setup();
            State = StoryState.Running;
            OnStarted?.Invoke(this, EventArgs.Empty);
            Begin();
        }

        protected void Finished()
        {
            State = StoryState.Stopped;
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
            State = StoryState.Stopped;
            CancellationTokenSource.Cancel();
        }

        public virtual void Pause()
        {
            State = StoryState.Paused;
        }

        public virtual void Resume()
        {
            State = StoryState.Running;
        }

        public bool IsRunning()
        {
            return State == StoryState.Running;
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