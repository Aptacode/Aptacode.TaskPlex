using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    [TestFixture]
    public class DoubleTransformationTests
    {
        TestRectangle _testRectangle;

        private static object[] _sourceLists = {
            new object[] {0, 1, new List<double> { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 }},
            new object[] {0, -1, new List<double> { -0.1, -0.2, -0.3, -0.4, -0.5, -0.6, -0.7, -0.8, -0.9, -1.0 } },
            new object[] {1, 1, new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } }
            };

        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }

        [Test, TestCaseSource("_sourceLists")]
        public void DoubleTransformation_OutputMatchesExpectedValues(double startValue, double endValue, List<double> expectedChangeLog)
        {
            PropertyTransformation transformation = PropertyTransformationHelpers.GetDoubleTransformation(_testRectangle, "Opacity", startValue, endValue, 10, 1);

            List<double> actualChangeLog = new List<double>();
            _testRectangle.OnOpacityChanged += (s, e) =>
            {
                actualChangeLog.Add(e.NewValue);
            };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()));
        }

    }
}
