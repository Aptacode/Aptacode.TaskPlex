using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.TaskPlex.Core_Tests.Utilites;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.TaskPlex.Core_Tests
{

    public class ColorTransformation_Tests
    {
        private static object[] _sourceLists = {
            new object[] { Color.FromArgb(0,0,0,0), Color.FromArgb(30,30,30,30), new List<Color> { Color.FromArgb(10, 10, 10, 10), Color.FromArgb(20, 20, 20, 20), Color.FromArgb(30, 30, 30, 30) } },
            new object[] { Color.FromArgb(30,30,30,30), Color.FromArgb(0,0,0,0), new List<Color> { Color.FromArgb(20, 20, 20, 20), Color.FromArgb(10, 10, 10, 10), Color.FromArgb(0, 0, 0, 0) } },
            new object[] { Color.FromArgb(255,0,30,10), Color.FromArgb(255,30,0,10), new List<Color> { Color.FromArgb(255, 10, 20, 10), Color.FromArgb(255, 20, 10, 10), Color.FromArgb(255, 30, 0, 10) } }
        };

        [Test, TestCaseSource("_sourceLists")]
        public void ColorComponentsChangingInDifferentDirections(Color startValue, Color endValue, List<Color> expectedChangeLog)
        {
            TestRectangle testRectangle = new TestRectangle();

            ColorTransformation transformation = PropertyTransformation_Helpers.GetColorTransformation(testRectangle, "BackgroundColor", startValue, endValue, 3, 1);

            List<Color> actualChangeLog = new List<Color>();
            testRectangle.OnBackgroundChanged += (s, e) =>
            {
                actualChangeLog.Add(e.NewValue);
            };

            transformation.StartAsync().Wait();
            Assert.That(actualChangeLog.SequenceEqual(expectedChangeLog));
        }
    }
}
