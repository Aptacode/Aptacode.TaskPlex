using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilities;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations.Interpolation
{
    [TestFixture]
    public class EaserTests
    {
        [TestCaseSource(typeof(Data.TestCaseData), "GetEaserData")]
        public void EaserProgresFrom0To1(Easer easer, List<double> expectedValues)
        {
            var comparer = new DoubleComparer();
            for (var i = 0; i < expectedValues.Count; i++)
            {
                Assert.That(comparer.Equals(expectedValues[i], easer.ProgressAt(i, 10)));
            }
        }
    }
}