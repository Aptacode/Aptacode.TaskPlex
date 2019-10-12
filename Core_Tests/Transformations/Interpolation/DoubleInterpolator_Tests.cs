using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Core_Tests.Utilites;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.Core.Tasks.Transformation.Inerpolator.Easing;

namespace Aptacode.TaskPlex.Core_Tests
{
    [TestFixture]
    public class DoubleEasing_Tests
    {
        private static object[] _sourceLists = {
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
        public void DoubleInterpolator_OutputValuesMatchSequence(double startValue, double endValue, List<double> expectedChangeLog, IEaser easer)
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
