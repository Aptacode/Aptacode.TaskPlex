using System;
using System.Collections.Generic;
using System.Threading;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.OldTests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.OldTests
{
    public class ParallelGroupTaskTests
    {
        [Test]
        public void EnsureThatParallelTasksAreStartedAtTheSameTime()
        {
            var task1StartTime = DateTime.Now;
            var task2StartTime = DateTime.Now;
            var task3StartTime = DateTime.Now;

            var tasks = new List<BaseTask>();
            var task1 = TaskPlexFactory.GetWaitTask(1);
            var task2 = TaskPlexFactory.GetWaitTask(1);
            var task3 = TaskPlexFactory.GetWaitTask(1);
            tasks.Add(task1);
            tasks.Add(task2);
            tasks.Add(task3);

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task2.OnStarted += (s, e) => { task2StartTime = DateTime.Now; };
            task3.OnStarted += (s, e) => { task3StartTime = DateTime.Now; };

            var groupTask = TaskPlexFactory.GetParallelGroup(tasks);

            groupTask.StartAsync(new CancellationTokenSource()).Wait();


            Assert.That(Math.Abs((task1StartTime - task2StartTime).TotalMilliseconds) <= 2);
            Assert.That(Math.Abs((task2StartTime - task3StartTime).TotalMilliseconds) <= 2);
        }
    }
}