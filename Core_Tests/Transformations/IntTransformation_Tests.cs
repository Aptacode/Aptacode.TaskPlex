using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.TaskPlex.Core_Tests.Utilites;

namespace Aptacode.TaskPlex.Core_Tests
{
    [TestFixture]
    public class IntTransformation_Tests
    {
        TestRectangle testRectangle;

        private static object[] _sourceLists = {
            new object[] {0, 100, new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 } },
            new object[] {0, -100, new List<int> { -10, -20, -30, -40, -50, -60, -70, -80, -90, -100 } },
            new object[] {1, 1, new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }}
            };

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        [Test, TestCaseSource("_sourceLists")]
        public void IntInterpolation_OutputMatchesExpectedValues(int startValue, int endValue, List<int> expectedChangeLog)
        {
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Width", startValue, endValue, 10, 1);

            List<int> actualChangeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                actualChangeLog.Add(e.NewValue);
            };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}
