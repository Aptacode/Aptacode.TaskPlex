using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    public class TaskTests
    {

        [TestCaseSource(typeof(TaskPlexTestData), "GetNormalTaskExamples")]
        public void StartFinishEventOrder(BaseTask task)
        {
            bool startedEventCalled = false;
            bool FinishedEventCalled = false;
            var startTime = DateTime.Now;
            var finishTime = DateTime.Now;
            task.OnStarted += (s, e) =>
            {
                startedEventCalled = true;
                startTime = DateTime.Now;
            };

            task.OnFinished += (s, e) =>
            {
                FinishedEventCalled = true;
                finishTime = DateTime.Now;
            };

            task.StartAsync().Wait();
            Assert.That(startedEventCalled);
            Assert.That(FinishedEventCalled);
            Assert.That(startTime < finishTime);
        }

        [Test, MaxTime(100)]
        [TestCaseSource(typeof(TaskPlexTestData), "GetNormalTaskExamples")]
        public void CanCancelTask(BaseTask task)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            task.OnCancelled += (s, e) =>
            {
                tcs.SetResult(true);
            };

            task.StartAsync();
            task.Cancel();

            tcs.Task.Wait();

            Assert.Pass();
        }
    }
}
