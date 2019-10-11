using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.Core.Tasks.Transformation.Inerpolator.Easing;

namespace Aptacode.TaskPlex.Core_Tests
{
    [TestFixture]
    public class IntInterpolation_Tests
    {
        private static object[] _sourceLists = {
            new object[] {0, 100, new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 } , new LinearEaser()},
            new object[] {0, -100, new List<int> { -10, -20, -30, -40, -50, -60, -70, -80, -90, -100 } , new LinearEaser()},
            new object[] {1, 1, new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } , new LinearEaser()}
            };

        [SetUp]
        public void Setup()
        {

        }

        [Test, TestCaseSource("_sourceLists")]
        public void IntInterpolator_OutputMatchesExpectedValues(int startValue, int endValue, List<int> expectedChangeLog, IEaser easer)
        {
            IntInterpolator interpolator = new IntInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(1));
            interpolator.SetEaser(easer);

            List<int> actualChangeLog = new List<int>();
            interpolator.OnValueChanged += (s, e) =>
            {
                actualChangeLog.Add(e.Value);
            };

            interpolator.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}
