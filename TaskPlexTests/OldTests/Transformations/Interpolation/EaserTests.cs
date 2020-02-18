using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;
using Aptacode.TaskPlex.Tests.OldTests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.OldTests.Transformations.Interpolation
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