using System;
using System.Collections.Generic;
using System.Threading;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    public class SequentialGroupTaskTests
    {
        [Test]
        public void SequentialGroupOrder()
        {
            var task1StartTime = DateTime.Now;
            var task1EndTime = DateTime.Now;
            var task2StartTime = DateTime.Now;
            var task2EndTime = DateTime.Now;

            var tasks = new List<BaseTask>();
            var task1 = TaskPlexFactory.GetWaitTask();
            var task2 = TaskPlexFactory.GetWaitTask();
            tasks.Add(task1);
            tasks.Add(task2);

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task1.OnFinished += (s, e) => { task1EndTime = DateTime.Now; };
            task2.OnStarted += (s, e) => { task2StartTime = DateTime.Now; };
            task2.OnFinished += (s, e) => { task2EndTime = DateTime.Now; };

            var groupTask = TaskPlexFactory.GetSequentialGroup(tasks);

            groupTask.StartAsync(new CancellationTokenSource()).Wait();

            Assert.That(task1StartTime < task1EndTime);
            Assert.That(task1EndTime < task2StartTime);
            Assert.That(task2StartTime < task2EndTime);
        }
    }
}