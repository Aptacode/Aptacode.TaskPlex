using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations.Interpolation.CubicEaser
{
    [TestFixture]
    public class CubicEaser_Tests
    {
        Easer easer;

        private static readonly object[] _sourceLists = {
            new object[] {new List<double> { 0, 0.01, 0.04, 0.09, 0.16, 0.25, 0.36, 0.49, 0.64, 0.81, 1 }},
            };

        private static readonly object[] _sourceLists2 = {
            new object[] {new List<double> { 0, 0.31622, 0.44721, 0.54772, 0.6324, 0.7071, 0.77454, 0.8366, 0.8944, 0.9486, 1 }},
            };


        [Test, TestCaseSource("_sourceLists")]
        public void Interpolation_Expected_Change_Log(List<double> expectedValues)
        {
            easer = new CubicInEaser();

            var comparer = new DoubleComparer();
            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.That(comparer.Equals(expectedValues[i], easer.ProgressAt(i, 10)));
            }
        }

        [Test, TestCaseSource("_sourceLists2")]
        public void CubeOutEaseFuncton(List<double> expectedValues)
        {
            easer = new CubicOutEaser();

            var comparer = new DoubleComparer();
            for (int i = 0; i < expectedValues.Count; i++)
            {
                Assert.That(comparer.Equals(expectedValues[i], easer.ProgressAt(i, 10)));
            }
        }

    }
}
