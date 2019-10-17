using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    [TestFixture]
    public class IntTransformationTests
    {
        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }

        private TestRectangle _testRectangle;

        private static object[] _sourceLists =
        {
        };

        [TestCaseSource(typeof(TaskPlexTestData), "GetIntInterpolationData")]
        public void IntInterpolation_OutputMatchesExpectedValues(int startValue, int endValue,
            List<int> expectedChangeLog)
        {
            var transformation =
                TaskPlexFactory.GetIntTransformation(_testRectangle, "Width", startValue, endValue, 10, 1);

            var actualChangeLog = new List<int>();
            _testRectangle.OnWidthChange += (s, e) => { actualChangeLog.Add(e.NewValue); };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}