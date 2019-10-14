using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations.Interpolation
{
    [TestFixture]
    public class DoubleEasing_Tests
    {
        private static readonly object[] _sourceLists = {
            new object[] {0, 1, new List<double> { 0, 0, 0, 0.1, 0.2, 0.3, 0.4, 0.6, 0.8, 1.0 }, new CubicInEaser()},
            new object[] {0, -1, new List<double> { 0, 0, 0, -0.1, -0.2, -0.3, -0.4, -0.6, -0.8, -1.0 }, new CubicInEaser()},
            new object[] {1, 1, new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } , new CubicInEaser()},
            new object[] {0, 1, new List<double> { 0.3, 0.4, 0.5, 0.6, 0.7, 0.7, 0.8, 0.8, 0.9, 1.0 }, new CubicOutEaser() },
            new object[] {0, -1, new List<double> { -0.3, -0.4, -0.5, -0.6, -0.7, -0.7, -0.8, -0.8, -0.9, -1.0 }, new CubicOutEaser() },
            new object[] {1, 1, new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, new CubicOutEaser() },
            new object[] {0, 1, new List<double> { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 }, new LinearEaser() },
            new object[] {0, -1, new List<double> { -0.1, -0.2, -0.3, -0.4, -0.5, -0.6, -0.7, -0.8, -0.9, -1.0 } , new LinearEaser() },
            new object[] {1, 1, new List<double> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } , new LinearEaser() }
            };


        [Test, TestCaseSource("_sourceLists")]
        public void DoubleInterpolator_OutputValuesMatchSequence(double startValue, double endValue, List<double> expectedChangeLog, Easer easer)
        {
            DoubleInterpolator transformation = new DoubleInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(1));
            transformation.SetEaser(easer);

            List<double> actualChangeLog = new List<double>();
            transformation.OnValueChanged += (s, e) =>
            {
                actualChangeLog.Add(e.Value);
            };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()));
        }
    }
}
