using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    public class DummyTask : BaseTask
    {
        public readonly int HashCode;

        public DummyTask(TimeSpan duration, int hashCode) : base(duration)
        {
            HashCode = hashCode;
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is DummyTask task && task.GetHashCode() == HashCode;
        }

        protected override void Setup()
        {
        }

        public override void Dispose()
        {
        }

        protected override async Task InternalTask()
        {
            await Task.Delay(Duration).ConfigureAwait(false);
        }
    }

    [TestFixture]
    public class TaskCoordinatorTests
    {
        [Test]
        public void CanApplyTask()
        {
            var coordinator = new TaskCoordinator(new NullLoggerFactory());

            var task1 = new DummyTask(TimeSpan.FromMilliseconds(10), 1);
            var taskStarted = false;
            var taskFinished = false;

            task1.OnStarted += (s, e) => { taskStarted = true; };
            task1.OnFinished += (s, e) => { taskFinished = true; };

            coordinator.Apply(task1);

            Assert.That(() => taskStarted, Is.True.After(40, 10), "Task1 has started");
            Assert.That(() => taskFinished, Is.True.After(40, 10), "Task1 has finished");
        }

        [Test]
        public void CanPauseTasks()
        {
            var coordinator = new TaskCoordinator(new NullLoggerFactory());

            var testRectangle = new TestRectangle();
            var task1 = TaskPlexFactory.Create(testRectangle, "Width", 100,
                TimeSpan.FromMilliseconds(50), RefreshRate.High);

            var task1StartTime = DateTime.Now;
            var task1EndTime = DateTime.Now;

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task1.OnFinished += (s, e) => { task1EndTime = DateTime.Now; };

            var task = Task.Run(async () =>
            {
                coordinator.Apply(task1);
                Task.Delay(10).Wait();
                coordinator.Pause();
                Task.Delay(50).Wait();
                coordinator.Resume();
                Task.Delay(100).Wait();
            });

            task.Wait();

            //Assert
            Assert.Greater((task1EndTime - task1StartTime).Milliseconds, 100,
                "The transformation should finish over 100 ms after starting due to the delay");
        }

        [Test]
        public void CollidingTasksWait()
        {
            var coordinator = new TaskCoordinator(new NullLoggerFactory());

            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20), 1);
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20), 1);

            var task1StartTime = DateTime.Now;
            var task1EndTime = DateTime.Now;
            var task2StartTime = DateTime.Now;
            var task2EndTime = DateTime.Now;

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task1.OnFinished += (s, e) => { task1EndTime = DateTime.Now; };

            task2.OnStarted += (s, e) => { task2StartTime = DateTime.Now; };
            task2.OnFinished += (s, e) => { task2EndTime = DateTime.Now; };

            coordinator.Apply(task1);
            coordinator.Apply(task2);

            Assert.That(() => task1StartTime < task1EndTime, Is.True.After(50, 10), "Task1 StartTime < Task1 EndTime");
            Assert.That(() => task1EndTime < task2StartTime, Is.True.After(50, 10), "Task1 EndTime < Task2 StartTime");
            Assert.That(() => task2StartTime < task2EndTime, Is.True.After(50, 10), "Task2 StartTime < Task2 EndTime");
        }

        [Test]
        public void ParallelTasksExecuteAtTheSameTime()
        {
            var coordinator = new TaskCoordinator(new NullLoggerFactory());

            var task1 = new DummyTask(TimeSpan.FromMilliseconds(10), 1);
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(10), 2);
            var groupTask = TaskPlexFactory.Parallel(task1, task2);

            var task1StartTime = DateTime.Now;
            var task1EndTime = DateTime.Now;
            var task2StartTime = DateTime.Now;
            var task2EndTime = DateTime.Now;
            var groupTaskStartTime = DateTime.Now;
            var groupTaskEndTime = DateTime.Now;

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task1.OnFinished += (s, e) => { task1EndTime = DateTime.Now; };

            task2.OnStarted += (s, e) => { task2StartTime = DateTime.Now; };
            task2.OnFinished += (s, e) => { task2EndTime = DateTime.Now; };

            groupTask.OnStarted += (s, e) => { groupTaskStartTime = DateTime.Now; };
            groupTask.OnFinished += (s, e) => { groupTaskEndTime = DateTime.Now; };

            coordinator.Apply(groupTask);

            Assert.That(() => groupTaskStartTime < task1StartTime, Is.True.After(20, 10),
                "Task1 StartTime < Task2 EndTime");
            Assert.That(() => task1StartTime < task2EndTime, Is.True.After(20, 10), "Task1 StartTime < Task2 EndTime");
            Assert.That(() => task2StartTime < task1EndTime, Is.True.After(20, 10),
                "Task2 StartTime < Task1 StartTime");

            Assert.That(() => task1EndTime < groupTaskEndTime, Is.True.After(20, 10),
                "Task1 EndTime < groupTask EndTime");
            Assert.That(() => task2EndTime < groupTaskEndTime, Is.True.After(20, 10),
                "Task2 EndTime < groupTask EndTime");
        }

        [Test]
        public void SequentialTasksExecuteOneAfterAnother()
        {
            var coordinator = new TaskCoordinator(new NullLoggerFactory());

            var task1 = new DummyTask(TimeSpan.FromMilliseconds(10), 1);
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(10), 2);
            var groupTask = TaskPlexFactory.Sequential(task1, task2);

            var task1StartTime = DateTime.Now;
            var task1EndTime = DateTime.Now;
            var task2StartTime = DateTime.Now;
            var task2EndTime = DateTime.Now;

            var groupStartTime = DateTime.Now;
            var groupEndTime = DateTime.Now;

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task1.OnFinished += (s, e) => { task1EndTime = DateTime.Now; };

            task2.OnStarted += (s, e) => { task2StartTime = DateTime.Now; };
            task2.OnFinished += (s, e) => { task2EndTime = DateTime.Now; };

            groupTask.OnStarted += (s, e) => { groupStartTime = DateTime.Now; };
            groupTask.OnFinished += (s, e) => { groupEndTime = DateTime.Now; };


            coordinator.Apply(groupTask);

            Assert.That(() => groupStartTime < task1StartTime, Is.True.After(40, 10),
                "Group StartTime < Task1 StartTime");
            Assert.That(() => task1StartTime < task1EndTime, Is.True.After(40, 10), "Task1 StartTime < Task1 EndTime");
            Assert.That(() => task1EndTime < task2StartTime, Is.True.After(40, 10), "Task1 EndTime < Task2 StartTime");
            Assert.That(() => task2StartTime < task2EndTime, Is.True.After(40, 10), "Task2 StartTime < Task2 EndTime");
            Assert.That(() => task2EndTime < groupEndTime, Is.True.After(40, 10), "Task2 EndTime < group EndTime");
        }
    }
}