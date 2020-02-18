using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.OldTests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.OldTests.Transformations
{
    public class ColorTransformationTests
    {
        [TestCaseSource(typeof(Data.TestCaseData), "GetColorInterpolationData")]
        public void ColorComponentsChangingInDifferentDirections(Color startValue, Color endValue,
            List<Color> expectedChangeLog)
        {
            var actualChangeLog = new List<Color>();

            var testRectangle = new TestRectangle { BackgroundColor = startValue };


            var transformation = new ColorTransformation(testRectangle, "BackgroundColor", () => startValue, () => endValue,
                color => { actualChangeLog.Add(color); }, TimeSpan.FromMilliseconds(3), RefreshRate.Highest);


            transformation.StartAsync(new CancellationTokenSource()).Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}