using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    [TestFixture]
    public class IntTransformationTests
    {
        TestRectangle _testRectangle;

        private static object[] _sourceLists = {
            new object[] {0, 100, new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 } },
            new object[] {0, -100, new List<int> { -10, -20, -30, -40, -50, -60, -70, -80, -90, -100 } },
            new object[] {1, 1, new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }}
            };

        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }

        [Test, TestCaseSource("_sourceLists")]
        public void IntInterpolation_OutputMatchesExpectedValues(int startValue, int endValue, List<int> expectedChangeLog)
        {
            PropertyTransformation transformation = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", startValue, endValue, 10, 1);

            List<int> actualChangeLog = new List<int>();
            _testRectangle.OnWidthChange += (s, e) =>
            {
                actualChangeLog.Add(e.NewValue);
            };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}
