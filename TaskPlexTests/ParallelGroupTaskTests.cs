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
        public void EnsureThatParallelTasksAreStartedAtTheSameTime()
        {
            var task1StartTime = DateTime.Now;
            var task2StartTime = DateTime.Now;
            var task3StartTime = DateTime.Now;

            var tasks = new List<IBaseTask>();
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

            groupTask.StartAsync().Wait();


            Assert.That(Math.Abs((task1StartTime - task2StartTime).TotalMilliseconds) <= 1 );
            Assert.That(Math.Abs((task2StartTime - task3StartTime).TotalMilliseconds) <= 1 );
        }
        
        
    }
}