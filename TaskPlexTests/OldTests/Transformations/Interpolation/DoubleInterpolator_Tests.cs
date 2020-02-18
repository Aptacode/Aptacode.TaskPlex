using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;
using Aptacode.TaskPlex.Tests.OldTests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.OldTests.Transformations.Interpolation
{
    [TestFixture]
    public class DoubleEasingTests
    {
        [TestCaseSource(typeof(Data.TestCaseData), "GetExpectedEaserData")]
        public void DoubleInterpolator_OutputValuesMatchSequence(double startValue, double endValue,
            List<double> expectedChangeLog, Easer easer)
        {
            var transformation = new DoubleInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(10),
                RefreshRate.Highest, easer);

            var actualChangeLog = new List<double>();
            transformation.OnValueChanged += (s, e) => { actualChangeLog.Add(e.Value); };

            transformation.StartAsync(new CancellationTokenSource()).Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()));
        }
    }
}