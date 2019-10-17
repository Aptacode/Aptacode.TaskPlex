using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            var testRectangle = new TestRectangle();
            var transformation =
                TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", startValue, endValue, 3, 1);
            var actualChangeLog = new List<Color>();

            testRectangle.OnBackgroundChanged += (s, e) => { actualChangeLog.Add(e.NewValue); };

            transformation.StartAsync().Wait();

            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}