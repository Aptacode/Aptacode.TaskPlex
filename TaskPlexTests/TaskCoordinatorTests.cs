using System;
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
            _taskCoordinator = new TaskCoordinator(new NullLoggerFactory(), _dummyUpdater);
            _taskCoordinator.Start();
        }

        private TaskCoordinator _taskCoordinator;
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
            _taskCoordinator.Apply(task1);
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
            _taskCoordinator.Apply(task1);
            _taskCoordinator.Apply(task2);

            _dummyUpdater.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _taskCoordinator.Pause();
            _dummyUpdater.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _taskCoordinator.Resume();
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
            _taskCoordinator.Apply(task1);
            _taskCoordinator.Apply(task2);

            _dummyUpdater.Update();
            Assert.That(() => task1.HasFinished || task2.HasFinished, Is.False.After(20, 5));
            _taskCoordinator.Pause(task1);
            _dummyUpdater.Update();
            Assert.That(() => !task1.HasFinished && task2.HasFinished, Is.True.After(20, 5));
            _taskCoordinator.Resume();
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
            _taskCoordinator.Apply(task1);
            _taskCoordinator.Apply(task2);

            _taskCoordinator.Stop();
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
            _taskCoordinator.Apply(task1);
            _taskCoordinator.Apply(task2);

            _taskCoordinator.Stop(task1);
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
            var groupTask = TaskPlexFactory.Parallel(task1, task2);

            //Todo
        }

        [Test]
        public void SequentialTasksExecuteOneAfterAnother()
        {
            //Arrange
            var task1 = new DummyTask(TicksToDuration(1));
            var task2 = new DummyTask(TicksToDuration(1));
            var groupTask = TaskPlexFactory.Sequential(task1, task2);

            //Todo
        }
    }
}