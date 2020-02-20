using System;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{

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
    }
}
