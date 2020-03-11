using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Stories;
using Microsoft.Extensions.Logging;

namespace Aptacode.TaskPlex
{
    /// <summary>
    ///     Manages the execution of tasks
    /// </summary>
    public class StoryManager : IStoryManager
    {
        private readonly ILogger _logger;
        private readonly RefreshRate _refreshRate;
        private readonly ConcurrentDictionary<BaseStory, object> _runningStories;

        /// <summary>
        ///     Manages the execution of tasks
        /// </summary>
        public StoryManager(ILoggerFactory loggerFactory, RefreshRate refreshRate)
        {
            _refreshRate = refreshRate;

            _logger = loggerFactory.CreateLogger<StoryManager>();
            _logger.LogTrace("Initializing PlexEngine");
            _runningStories = new ConcurrentDictionary<BaseStory, object>();
        }

        public IQueryable<BaseStory> GetAll()
        {
            return _runningStories.Keys.AsQueryable();
        }

        public void Stop(BaseStory story)
        {
            if (_runningStories.ContainsKey(story))
            {
                story.Cancel();
            }
        }

        public void Pause(BaseStory story)
        {
            if (_runningStories.ContainsKey(story))
            {
                story.Pause();
            }
        }

        public void Resume(BaseStory story)
        {
            if (_runningStories.ContainsKey(story))
            {
                story.Resume();
            }
        }

        public void Stop()
        {
            foreach (var story in _runningStories.Keys)
            {
                story.Cancel();
            }
        }

        public void Pause()
        {
            foreach (var story in _runningStories.Keys)
            {
                story.Pause();
            }
        }

        public void Resume()
        {
            foreach (var story in _runningStories.Keys)
            {
                story.Resume();
            }
        }

        /// <summary>
        ///     Start executing a given task
        /// </summary>
        /// <param name="story"></param>
        public void Apply(BaseStory story)
        {
            //Return null if the tasks given was null
            if (story == null)
            {
                return;
            }

            _logger.LogTrace($"Applying task: {story}");
            if (_runningStories.ContainsKey(story))
            {
                story.Reset();
            }
            else
            {
                //Add the task to the list for updating
                _runningStories.TryAdd(story, 0);

                //When the task is finished remove it from the list
                story.OnFinished += Task_OnFinished;
                story.OnCancelled += Task_OnFinished;
            }

            try
            {
                //Run the task asynchronously with the Coordinators cancellation token source and refresh rate
                story.Start(new CancellationTokenSource(), _refreshRate);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Task Failed: {ex}");
            }
        }

        public void Update()
        {
            foreach (var story in _runningStories.Keys)
            {
                story.Update();
            }
        }

        private void Task_OnFinished(object sender, EventArgs e)
        {
            if (!(sender is BaseStory finishedStory))
            {
                return;
            }

            finishedStory.OnFinished -= Task_OnFinished;
            finishedStory.OnCancelled -= Task_OnFinished;
            _runningStories.TryRemove(finishedStory, out _);
        }
    }
}