using System;
using Aptacode.TaskPlex.Enums;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    [TestFixture]
    public class StoryManagerTests
    {
        [SetUp]
        public void Setup()
        {
            _storyManager = new StoryManager(new NullLoggerFactory(), _refreshRate);
        }

        private readonly RefreshRate _refreshRate = RefreshRate.Normal;

        private StoryManager _storyManager;

        private TimeSpan TicksToDuration(int ticks)
        {
            return TimeSpan.FromMilliseconds(ticks * (int) _refreshRate);
        }

        [Test]
        public void CanApplyTask()
        {
            //Arrange
            var task1 = new DummyStory(TicksToDuration(1));

            //Apply
            _storyManager.Apply(task1);
            _storyManager.Update();

            //Assert
            Assert.That(() => task1.HasFinished, Is.True.After(30, 5));
        }


        [Test]
        public void CanPauseAllTasks()
        {
            //Arrange
            var task1 = new DummyStory(TicksToDuration(2));
            var task2 = new DummyStory(TicksToDuration(2));

            //Apply
            _storyManager.Apply(task1);
            _storyManager.Apply(task2);

            _storyManager.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _storyManager.Pause();
            _storyManager.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _storyManager.Resume();
            _storyManager.Update();
            Assert.That(() => task1.HasFinished && task2.HasFinished, Is.True.After(20, 5));
        }

        [Test]
        public void CanPauseSpecificTasks()
        {
            //Arrange
            var task1 = new DummyStory(TicksToDuration(2));
            var task2 = new DummyStory(TicksToDuration(2));

            //Apply
            _storyManager.Apply(task1);
            _storyManager.Apply(task2);

            _storyManager.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _storyManager.Pause(task1);
            _storyManager.Update();
            Assert.That(() => !task1.HasFinished && task2.HasFinished, Is.True.After(20, 5));
            _storyManager.Resume();
            _storyManager.Update();
            Assert.That(() => task1.HasFinished && task2.HasFinished, Is.True.After(20, 5));
        }

        [Test]
        public void CanReset()
        {
            //ToDo
        }

        [Test]
        public void CanStopAllTasks()
        {
            //Arrange
            var task1 = new DummyStory(TicksToDuration(1));
            var task2 = new DummyStory(TicksToDuration(1));

            //Apply
            _storyManager.Apply(task1);
            _storyManager.Apply(task2);

            _storyManager.Stop();
            _storyManager.Update();


            //Assert
            Assert.That(() => task1.HasCanceled, Is.True.After(30, 5));
            Assert.That(() => task2.HasCanceled, Is.True.After(30, 5));
        }

        [Test]
        public void CanStopSpecificTasks()
        {
            //Arrange
            var task1 = new DummyStory(TicksToDuration(1));
            var task2 = new DummyStory(TicksToDuration(1));

            //Apply
            _storyManager.Apply(task1);
            _storyManager.Apply(task2);

            _storyManager.Stop(task1);
            _storyManager.Update();


            //Assert
            Assert.That(() => task1.HasCanceled, Is.True.After(30, 5));
            Assert.That(() => task2.HasCanceled, Is.False.After(30, 5));
        }

        [Test]
        public void ParallelTasksExecuteAtTheSameTime()
        {
            //Arrange
            var task1 = new DummyStory(TicksToDuration(1));
            var task2 = new DummyStory(TicksToDuration(1));
            var groupTask = StoryBuilder.Parallel(task1, task2);

            //Todo
        }

        [Test]
        public void SequentialTasksExecuteOneAfterAnother()
        {
            //Arrange
            var task1 = new DummyStory(TicksToDuration(1));
            var task2 = new DummyStory(TicksToDuration(1));
            var groupTask = StoryBuilder.Sequential(task1, task2);

            //Todo
        }
    }
}