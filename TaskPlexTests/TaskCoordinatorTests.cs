using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    [TestFixture]
    public class TaskCoordinatorTests
    {
        [SetUp]
        public void Setup()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(loggingBuilder => { loggingBuilder.AddConsole(); })
                .BuildServiceProvider();

            _taskCoordinator = new TaskCoordinator(serviceProvider.GetService<ILoggerFactory>());
        }

        private TaskCoordinator _taskCoordinator;

        [Test]
        [MaxTime(300)]
        [TestCaseSource(typeof(Data.TestCaseData), "GetParallelTasks")]
        [TestCaseSource(typeof(Data.TestCaseData), "GetCollidingTasks")]
        public void TasksFinished(IEnumerable<BaseTask> tasks)
        {
            var tcs = new TaskCompletionSource<bool>();

            var finishedTaskCount = 0;

            var baseTasks = tasks.ToList();
            foreach (var baseTask in baseTasks)
            {
                baseTask.OnFinished += (s, e) =>
                {
                    if (++finishedTaskCount >= baseTasks.Count())
                    {
                        tcs.SetResult(true);
                    }
                };
            }

            foreach (var baseTask in baseTasks)
            {
                _taskCoordinator.Apply(baseTask);
            }

            tcs.Task.Wait();
            Assert.Pass();
        }

        [Test]
        [MaxTime(1000)]
        [TestCaseSource(typeof(Data.TestCaseData), "GetParallelTasks")]
        [TestCaseSource(typeof(Data.TestCaseData), "GetCollidingTasks")]
        public void TasksStarted(IEnumerable<BaseTask> tasks)
        {
            var tcs = new TaskCompletionSource<bool>();

            var startedTaskCount = 0;

            var baseTasks = tasks.ToList();
            foreach (var baseTask in baseTasks)
            {
                baseTask.OnStarted += (s, e) =>
                {
                    if (++startedTaskCount >= baseTasks.Count())
                    {
                        tcs.SetResult(true);
                    }
                };
            }

            foreach (var baseTask in baseTasks)
            {
                _taskCoordinator.Apply(baseTask);
            }

            tcs.Task.Wait();

            Assert.Pass();
        }
    }
}