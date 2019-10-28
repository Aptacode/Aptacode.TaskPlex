using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tests.Data;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations.Interpolation
{
    [TestFixture]
    public class IntInterpolationTests
    {
        [TestCaseSource(typeof(TaskPlexTestData), "GetLinearEaserData")]
        public void IntInterpolator_OutputMatchesExpectedValues(int startValue, int endValue,
            List<int> expectedChangeLog, Easer easer)
        {
            var interpolator = new IntInterpolator(startValue, endValue, TimeSpan.FromMilliseconds(10),
                TimeSpan.FromMilliseconds(1));
            interpolator.Easer = easer;

            var actualChangeLog = new List<int>();
            interpolator.OnValueChanged += (s, e) => { actualChangeLog.Add(e.Value); };

            interpolator.StartAsync(new CancellationTokenSource()).Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}