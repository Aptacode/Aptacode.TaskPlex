using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations.Interpolation
{
    [TestFixture]
    public class IntInterpolationTests
    {
        private static object[] _sourceLists = {
            new object[] {0, 100, new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 } , new LinearEaser()},
            new object[] {0, -100, new List<int> { -10, -20, -30, -40, -50, -60, -70, -80, -90, -100 } , new LinearEaser()},
            new object[] {1, 1, new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } , new LinearEaser()}
            };


        [Test, TestCaseSource("_sourceLists")]
        public void IntInterpolator_OutputMatchesExpectedValues(int startValue, int endValue, List<int> expectedChangeLog, Easer easer)
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
