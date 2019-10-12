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
    public class DoubleTransformation_Tests
    {
        TestRectangle testRectangle;

        private static object[] _sourceLists = {
            new object[] {0, 1, new List<double> { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 }},
            new object[] {0, -1, new List<double> { -0.1, -0.2, -0.3, -0.4, -0.5, -0.6, -0.7, -0.8, -0.9, -1.0 } },
            new object[] {1, 1, new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } }
            };

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        [Test, TestCaseSource("_sourceLists")]
        public void DoubleTransformation_OutputMatchesExpectedValues(double startValue, double endValue, List<double> expectedChangeLog)
        {
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetDoubleTransformation(testRectangle, "Opacity", startValue, endValue, 10, 1);

            List<double> actualChangeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                actualChangeLog.Add(e.NewValue);
            };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()));
        }

    }
}
