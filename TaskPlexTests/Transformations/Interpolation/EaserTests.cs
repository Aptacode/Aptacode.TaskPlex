using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations.Interpolation
{
    [TestFixture]
    public class EaserTests
    {
        [TestCaseSource(typeof(TaskPlexTestData), "GetEaserData")]

        public void EaserProgresFrom0To1(Easer easer, List<double> expectedValues)
        {
            var comparer = new DoubleComparer();
            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.That(comparer.Equals(expectedValues[i], easer.ProgressAt(i, 10)));
            }
        }
    }
}
