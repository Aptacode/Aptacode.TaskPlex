using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    public class TaskTests
    {
        [Test]
        [MaxTime(100)]
        [TestCaseSource(typeof(TaskPlexTestData), "GetNormalTaskExamples")]
        public void CanCancelTask(BaseTask task)
        {
            new TaskFactory().StartNew(() =>
            {
                Assert.Catch(() => { task.StartAsync().Wait(); });
            });

            task.Cancel();

            Assert.Pass();
        }
    }
}