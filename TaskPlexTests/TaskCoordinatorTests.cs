using System;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
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

        public override bool Equals(object obj) => obj is DummyTask task && task.GetHashCode() == HashCode;

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

            var task1 = TaskPlexFactory.Wait(TimeSpan.FromMilliseconds(1));
            var taskStarted = false;
            var taskFinished = false;

            task1.OnStarted += (s, e) => { taskStarted = true; };
            task1.OnFinished += (s, e) => { taskFinished = true; };

            coordinator.Apply(task1);

            Assert.That(() => taskStarted, Is.True.After(40, 10));
            Assert.That(() => taskFinished, Is.True.After(40, 10));
        }

        [Test]
        public void CollidingTasksWait()
        {
            var coordinator = new TaskCoordinator(new NullLoggerFactory());

            var task1 = new DummyTask(TimeSpan.FromMilliseconds(10), 1);
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(10), 1);

            DateTime task1StartTime = DateTime.Now;
            DateTime task1EndTime = DateTime.Now;
            DateTime task2StartTime = DateTime.Now;
            DateTime task2EndTime = DateTime.Now;

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task1.OnFinished += (s, e) => { task1EndTime = DateTime.Now; };

            task2.OnStarted += (s, e) => { task2StartTime = DateTime.Now; };
            task2.OnFinished += (s, e) => { task2EndTime = DateTime.Now; };

            coordinator.Apply(task1);
            coordinator.Apply(task2);

            Assert.That(() => task1StartTime < task1EndTime, Is.True.After(40, 10), "Task1 StartTime < Task1 EndTime");
            Assert.That(() => task1EndTime < task2StartTime, Is.True.After(40, 10), "Task1 EndTime < Task2 StartTime");
            Assert.That(() => task2StartTime < task2EndTime, Is.True.After(40, 10), "Task2 StartTime < Task2 EndTime");
        }
    }
}