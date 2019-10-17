using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    [TestFixture]
    public class DoubleTransformationTests
    {
        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }

        private TestRectangle _testRectangle;

        [TestCaseSource(typeof(TaskPlexTestData), "GetDoubleInterpolationData")]
        public void DoubleTransformation_OutputMatchesExpectedValues(double startValue, double endValue,
            List<double> expectedChangeLog)
        {
            var transformation =
                TaskPlexFactory.GetDoubleTransformation(_testRectangle, "Opacity", startValue, endValue, 10, 1);

            var actualChangeLog = new List<double>();
            _testRectangle.OnOpacityChanged += (s, e) => { actualChangeLog.Add(e.NewValue); };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()));
        }
    }
}