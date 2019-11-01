using System.Threading;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    public class PropertyTransformationTests
    {
        [Test]
        [TestCaseSource(typeof(TaskPlexTestData), "GetNonZeroTransformationAndInterval")]
        [TestCaseSource(typeof(TaskPlexTestData), "GetInstantTransformations")]
        [TestCaseSource(typeof(TaskPlexTestData), "GetZeroIntervalTransformations")]
        public void TargetsPropertyMatchesExpectedValue(PropertyTransformation task, object expectedEndValue)
        {
            task.StartAsync(new CancellationTokenSource()).Wait();
            var property = task.Target.GetType().GetProperty(task.Property);

            Assert.AreEqual(expectedEndValue, property?.GetValue(task.Target));
        }
    }
}