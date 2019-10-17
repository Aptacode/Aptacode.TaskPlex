using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    public class PropertyTransformationTests
    {

        private TaskCoordinator _taskCoordinator;

        [SetUp]
        public void Setup()
        {
            _taskCoordinator = new TaskCoordinator();
        }

        [Test, MaxTime(1000)]
        [TestCaseSource(typeof(TaskPlexTestData), "GetParallelTasks")]
        public void TasksStartedAndFinished(IEnumerable<BaseTask> tasks)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            var startedTaskCount = 0;
            var finishedTaskCount = 0;

            foreach (var baseTask in tasks)
            {
                baseTask.OnStarted += (s, e) => { startedTaskCount++; };
                baseTask.OnFinished += (s, e) =>
                {
                    if (++finishedTaskCount >= tasks.Count())
                        tcs.SetResult(true);
                };
            }

            foreach (var baseTask in tasks)
            {
                _taskCoordinator.Apply(baseTask);
            }

            tcs.Task.Wait();
            Assert.Pass();
        }

        [Test, MaxTime(1000)]
        [TestCaseSource(typeof(TaskPlexTestData), "GetCollidingTasks")]
        public void Colliding_Transformations(IEnumerable<BaseTask> tasks)
        {

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            var startTimes = new List<DateTime>();
            var endTimes = new List<DateTime>();
            var finishedTaskCount = 0;

            foreach (var baseTask in tasks)
            {
                baseTask.OnStarted += (s, e) => { startTimes.Add(DateTime.Now); };
                baseTask.OnFinished += (s, e) => { };
                baseTask.OnFinished += (s, e) =>
                {
                    endTimes.Add(DateTime.Now);
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


            for (int i = 1; i < endTimes.Count(); i++)
            {
                Assert.That(endTimes[i - 1] < startTimes[i]);
            }

        }
    }
}