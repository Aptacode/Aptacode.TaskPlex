using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks
{
    public class RepeatTask : BaseTask
    {
        public RepeatTask(BaseTask child, int count) : base(child.Duration)
        {
            Child = child;
            Count = count;
        }

        public BaseTask Child { get; }
        public int Count { get; set; }

        public override void Pause()
        {
            Child.Pause();
            base.Pause();
        }

        public override void Resume()
        {
            Child.Resume();
            base.Resume();
        }

        public override void Update()
        {
            Child.Update();
        }

        protected override async Task InternalTask()
        {
            var repeatCount = 0;

            while (++repeatCount <= Count && !CancellationTokenSource.IsCancellationRequested)
            {
                await Child.StartAsync(CancellationTokenSource, RefreshRate).ConfigureAwait(false);
            }
        }
    }
}