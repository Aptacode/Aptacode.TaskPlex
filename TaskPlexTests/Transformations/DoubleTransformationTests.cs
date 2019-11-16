using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilities;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    [TestFixture]
    public class DoubleTransformationTests
    {
        [SetUp]
        public void Setup() => _testRectangle = new TestRectangle();

        private TestRectangle _testRectangle;

        [TestCaseSource(typeof(Data.TestCaseData), "GetDoubleInterpolationData")]
        public void DoubleTransformation_OutputMatchesExpectedValues(double startValue, double endValue,
            List<double> expectedChangeLog)
        {
            var transformation =
                TaskPlexFactory.GetDoubleTransformation(_testRectangle, "Opacity", startValue, endValue, 10, 1);

            var actualChangeLog = new List<double>();
            _testRectangle.OnOpacityChanged += (s, e) => { actualChangeLog.Add(e.NewValue); };

            transformation.StartAsync(new CancellationTokenSource()).Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()));
        }
    }
}