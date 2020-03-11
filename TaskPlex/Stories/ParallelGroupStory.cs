using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Enums;

namespace Aptacode.TaskPlex.Stories
{
    public class ParallelGroupStory : GroupStory
    {
        private int _completedStoryCount;

        /// <summary>
        ///     Execute the specified tasks in parallel
        /// </summary>
        public ParallelGroupStory(List<BaseStory> stories) : base(stories.Max(story => story.Duration), stories)
        {
        }

        public override void Pause()
        {
            Stories.ForEach(story => story.Pause());
            base.Pause();
        }

        public override void Resume()
        {
            Stories.ForEach(story => story.Resume());
            base.Resume();
        }

        protected override void Setup()
        {
            _completedStoryCount = 0;
            foreach (var story in Stories)
            {
                story.OnFinished += IsFinished;
                story.OnCancelled += IsFinished;
            }
        }

        protected override void Begin()
        {
            Stories.ForEach(story => story.Start(CancellationTokenSource, RefreshRate));
        }

        protected override void Cleanup()
        {
            _completedStoryCount = 0;

            foreach (var story in Stories)
            {
                story.OnFinished -= IsFinished;
                story.OnCancelled -= IsFinished;
            }
        }

        private void IsFinished(object sender, EventArgs args)
        {
            _completedStoryCount++;
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

            if (_completedStoryCount >= Stories.Count)
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