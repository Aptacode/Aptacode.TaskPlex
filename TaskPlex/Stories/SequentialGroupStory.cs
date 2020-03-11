using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Stories
{
    public class SequentialGroupStory : GroupStory
    {
        private int _finishedStoryCount;

        /// <summary>
        ///     Execute the specified tasks sequentially in the order they occur in the input list
        /// </summary>
        /// <param name="stories"></param>
        public SequentialGroupStory(List<BaseStory> stories) : base(TimeSpan.FromMilliseconds(stories.Aggregate(0,
            (current, story) => current + (int)story.Duration.TotalMilliseconds)), stories)
        {
        }

        public override void Pause()
        {
            Stories.ForEach(story => story.Pause());
        }

        public override void Resume()
        {
            Stories.ForEach(story => story.Resume());
        }

        protected override void Setup()
        {
            _finishedStoryCount = 0;
            foreach (var story in Stories)
            {
                story.OnFinished += BaseTask_OnFinished;
                story.OnCancelled += BaseTask_OnFinished;
            }
        }

        protected override void Begin()
        {
            Stories.First().Start(CancellationTokenSource, RefreshRate);
        }

        private void BaseTask_OnFinished(object sender, EventArgs e)
        {
            if (!(sender is BaseStory story))
            {
                return;
            }

            _finishedStoryCount++;

            var nextStoryIndex = Stories.IndexOf(story) + 1;
            if (nextStoryIndex < Stories.Count)
            {
                Stories[nextStoryIndex].Start(CancellationTokenSource, RefreshRate);
            }
        }

        protected override void Cleanup()
        {
            _finishedStoryCount = 0;

            foreach (var story in Stories)
            {
                story.OnFinished -= BaseTask_OnFinished;
                story.OnCancelled -= BaseTask_OnFinished;
            }
        }

        public override void Update()
        {
            if (IsCancelled)
            {
                Finished();
                return;
            }

            if (!IsRunning())
            {
                return;
            }

            if (_finishedStoryCount >= Stories.Count)
            {
                Finished();
            }

            Stories.ForEach(story => story.Update());
        }

        public override void Reset()
        {
            State = StoryState.Paused;
            Cleanup();
            Stories.ForEach(story => story.Reset());
        }
    }
}