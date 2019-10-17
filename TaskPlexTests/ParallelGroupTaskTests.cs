using System;
using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{

    public class ParallelGroupTaskTests
    {
        [Test]
        public void ParallelGroupOrder()
        {
            var task1StartTime = DateTime.Now;
            var task1EndTime = DateTime.Now;
            var task2StartTime = DateTime.Now;
            var task2EndTime = DateTime.Now;  
            var task3StartTime = DateTime.Now;
            var task3EndTime = DateTime.Now;

            List<BaseTask> tasks = new List<BaseTask>();
            var task1 = TaskPlexFactory.GetWaitTask(15);
            var task2 = TaskPlexFactory.GetWaitTask(10);
            var task3 = TaskPlexFactory.GetWaitTask(1);
            tasks.Add(task1);
            tasks.Add(task2);
            tasks.Add(task3);

            task1.OnStarted += (s, e) => { task1StartTime = DateTime.Now; };
            task1.OnFinished += (s, e) => { task1EndTime = DateTime.Now; };
            task2.OnStarted += (s, e) => { task2StartTime = DateTime.Now; };
            task2.OnFinished += (s, e) => { task2EndTime = DateTime.Now; };
            task3.OnStarted += (s, e) => { task3StartTime = DateTime.Now; };
            task3.OnFinished += (s, e) => { task3EndTime = DateTime.Now; };

            var groupTask = TaskPlexFactory.GetParallelGroup(tasks);

            groupTask.StartAsync().Wait();

            Assert.That(task3EndTime < task2EndTime);
            Assert.That(task2EndTime < task1EndTime);
        }
    }
}
