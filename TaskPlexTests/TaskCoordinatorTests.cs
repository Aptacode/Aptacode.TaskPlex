using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    [TestFixture]
    public class TaskCoordinatorTests
    {
        private TaskCoordinator _taskCoordinator;

        [SetUp]
        public void Setup()
        {
            _taskCoordinator = new TaskCoordinator();
        }

        [Test]
        [MaxTime(1000)]
        [TestCaseSource(typeof(TaskPlexTestData), "GetParallelTasks")]
        [TestCaseSource(typeof(TaskPlexTestData), "GetCollidingTasks")]
        public void TasksStarted(IEnumerable<IBaseTask> tasks)
        {
            var tcs = new TaskCompletionSource<bool>();

            var startedTaskCount = 0;

            foreach (var baseTask in tasks)
            {
                baseTask.OnStarted += (s, e) =>
                {
                    if (++startedTaskCount >= tasks.Count())
                    {
                        tcs.SetResult(true);
                    }
                };
            }

            foreach (var baseTask in tasks)
            {
                _taskCoordinator.Apply(baseTask);
            }

            tcs.Task.Wait();

            Assert.Pass();
        } 
        
        [Test]
        [MaxTime(100)]
        [TestCaseSource(typeof(TaskPlexTestData), "GetParallelTasks")]
        [TestCaseSource(typeof(TaskPlexTestData), "GetCollidingTasks")]
        public void TasksFinished(IEnumerable<IBaseTask> tasks)
        {
            var tcs = new TaskCompletionSource<bool>();

            var finishedTaskCount = 0;

            foreach (var baseTask in tasks)
            {
                baseTask.OnFinished += (s, e) =>
                {
                    if (++finishedTaskCount >= tasks.Count())
                    {
                        tcs.SetResult(true);
                    }
                };
            }

            foreach (var baseTask in tasks)
            {
                _taskCoordinator.Apply(baseTask);
            }

            tcs.Task.Wait();
            Assert.Pass();
        }
    }
}