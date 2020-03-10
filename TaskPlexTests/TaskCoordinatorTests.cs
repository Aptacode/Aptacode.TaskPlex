using System;
using Aptacode.TaskPlex.Engine;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    [TestFixture]
    public class TaskCoordinatorTests
    {
        [SetUp]
        public void Setup()
        {
            _dummyUpdater = new DummyUpdater();
            _plexEngine = new PlexEngine(new NullLoggerFactory(), _dummyUpdater);
            _plexEngine.Start();
        }

        private PlexEngine _plexEngine;
        private DummyUpdater _dummyUpdater;

        private TimeSpan TicksToDuration(int ticks)
        {
            return TimeSpan.FromMilliseconds(ticks * (int) _dummyUpdater.RefreshRate);
        }

        [Test]
        public void CanApplyTask()
        {
            //Arrange
            var task1 = new DummyTask(TicksToDuration(1));

            //Apply
            _plexEngine.Apply(task1);
            _dummyUpdater.Update();

            //Assert
            Assert.That(() => task1.HasFinished, Is.True.After(30, 5));
        }


        [Test]
        public void CanPauseAllTasks()
        {
            //Arrange
            var task1 = new DummyTask(TicksToDuration(2));
            var task2 = new DummyTask(TicksToDuration(2));

            //Apply
            _plexEngine.Apply(task1);
            _plexEngine.Apply(task2);

            _dummyUpdater.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _plexEngine.Pause();
            _dummyUpdater.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _plexEngine.Resume();
            _dummyUpdater.Update();
            Assert.That(() => task1.HasFinished && task2.HasFinished, Is.True.After(20, 5));
        }

        [Test]
        public void CanPauseSpecificTasks()
        {
            //Arrange
            var task1 = new DummyTask(TicksToDuration(2));
            var task2 = new DummyTask(TicksToDuration(2));

            //Apply
            _plexEngine.Apply(task1);
            _plexEngine.Apply(task2);

            _dummyUpdater.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _plexEngine.Pause(task1);
            _dummyUpdater.Update();
            Assert.That(() => !task1.HasFinished && task2.HasFinished, Is.True.After(20, 5));
            _plexEngine.Resume();
            _dummyUpdater.Update();
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
            var task1 = new DummyTask(TicksToDuration(1));
            var task2 = new DummyTask(TicksToDuration(1));

            //Apply
            _plexEngine.Apply(task1);
            _plexEngine.Apply(task2);

            _plexEngine.Stop();
            _dummyUpdater.Update();

            //Assert
            Assert.That(() => task1.HasCanceled, Is.True.After(30, 5));
            Assert.That(() => task2.HasCanceled, Is.True.After(30, 5));
        }

        [Test]
        public void CanStopSpecificTasks()
        {
            //Arrange
            var task1 = new DummyTask(TicksToDuration(1));
            var task2 = new DummyTask(TicksToDuration(1));

            //Apply
            _plexEngine.Apply(task1);
            _plexEngine.Apply(task2);

            _plexEngine.Stop(task1);
            _dummyUpdater.Update();

            //Assert
            Assert.That(() => task1.HasCanceled, Is.True.After(30, 5));
            Assert.That(() => task2.HasCanceled, Is.False.After(30, 5));
        }

        [Test]
        public void ParallelTasksExecuteAtTheSameTime()
        {
            //Arrange
            var task1 = new DummyTask(TicksToDuration(1));
            var task2 = new DummyTask(TicksToDuration(1));
            var groupTask = PlexFactory.Parallel(task1, task2);

            //Todo
        }

        [Test]
        public void SequentialTasksExecuteOneAfterAnother()
        {
            //Arrange
            var task1 = new DummyTask(TicksToDuration(1));
            var task2 = new DummyTask(TicksToDuration(1));
            var groupTask = PlexFactory.Sequential(task1, task2);

            //Todo
        }
    }
}