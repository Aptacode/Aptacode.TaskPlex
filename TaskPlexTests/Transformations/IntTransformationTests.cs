using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilities;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    [TestFixture]
    public class IntTransformationTests
    {
        private TestRectangle _testRectangle;

        [SetUp]
        public void Setup() => _testRectangle = new TestRectangle();



        [TestCaseSource(typeof(Data.TestCaseData), "GetIntInterpolationData")]
        public void IntInterpolation_OutputMatchesExpectedValues(int startValue, int endValue,
            List<int> expectedChangeLog)
        {
            var transformation =
                TaskPlexFactory.GetIntTransformation(_testRectangle, "Width", startValue, endValue, 10, 1);

            var actualChangeLog = new List<int>();
            _testRectangle.OnWidthChange += (s, e) => { actualChangeLog.Add(e.NewValue); };

            transformation.StartAsync(new CancellationTokenSource()).Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}