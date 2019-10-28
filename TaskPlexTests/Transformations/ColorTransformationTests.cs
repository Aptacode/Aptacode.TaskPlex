using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    public class ColorTransformationTests
    {
        [TestCaseSource(typeof(TaskPlexTestData), "GetColorInterpolationData")]
        public void ColorComponentsChangingInDifferentDirections(Color startValue, Color endValue,
            List<Color> expectedChangeLog)
        {
            var actualChangeLog = new List<Color>();

            var testRectangle = new TestRectangle();

            testRectangle.BackgroundColor = startValue;

            var transformation = new ColorTransformation(testRectangle, "BackgroundColor", () => endValue,
                color => { actualChangeLog.Add(color); }, TimeSpan.FromMilliseconds(3), TimeSpan.FromMilliseconds(1));


            transformation.StartAsync(new CancellationTokenSource()).Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}