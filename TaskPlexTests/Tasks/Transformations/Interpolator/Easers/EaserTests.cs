using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Tasks.Transformations.Interpolator.Easers
{
    /// <summary>
    ///     Checks the ProgressAt(int, int) methods for each built in easer function return the correct value
    ///     at for specified index and total
    /// </summary>
    [TestFixture]
    public class EaserTests
    {
        private static readonly LinearEaser LinearEaser = new LinearEaser();
        private static readonly SineEaser SineEaser = new SineEaser();
        private static readonly CubicInEaser CubicInEaser = new CubicInEaser();
        private static readonly CubicOutEaser CubicOutEaser = new CubicOutEaser();

        [Test]
        [TestCase(0, 0, 1)]
        [TestCase(0, 100, 0)]
        [TestCase(0, -1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(-1, 0, 0)]
        [TestCase(-1, 1, 0)]
        [TestCase(-1, 100, 0)]
        [TestCase(-1, -1, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 0.25)]
        [TestCase(1, 3, 0.11)]
        [TestCase(2, 3, 0.44)]
        [TestCase(0, 10, 0)]
        [TestCase(1, 10, 0.02)]
        [TestCase(2, 10, 0.04)]
        [TestCase(3, 10, 0.08)]
        [TestCase(4, 10, 0.16)]
        [TestCase(5, 10, 0.25)]
        [TestCase(6, 10, 0.36)]
        [TestCase(7, 10, 0.48)]
        [TestCase(8, 10, 0.64)]
        [TestCase(9, 10, 0.81)]
        [TestCase(10, 10, 1)]
        public void CubicInEaserTests(int index, int total, double expectedValue)
        {
            Assert.That(CubicInEaser.ProgressAt(index, total), Is.EqualTo(expectedValue).Within(0.01));
        }

        [Test]
        [TestCase(0, 0, 1)]
        [TestCase(0, 100, 0)]
        [TestCase(0, -1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(-1, 0, 0)]
        [TestCase(-1, 1, 0)]
        [TestCase(-1, 100, 0)]
        [TestCase(-1, -1, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 0.70)]
        [TestCase(1, 3, 0.57)]
        [TestCase(2, 3, 0.81)]
        [TestCase(0, 10, 0)]
        [TestCase(1, 10, 0.31)]
        [TestCase(2, 10, 0.44)]
        [TestCase(3, 10, 0.54)]
        [TestCase(4, 10, 0.63)]
        [TestCase(5, 10, 0.70)]
        [TestCase(6, 10, 0.77)]
        [TestCase(7, 10, 0.83)]
        [TestCase(8, 10, 0.90)]
        [TestCase(9, 10, 0.94)]
        [TestCase(10, 10, 1)]
        public void CubicOutEaserTests(int index, int total, double expectedValue)
        {
            Assert.That(CubicOutEaser.ProgressAt(index, total), Is.EqualTo(expectedValue).Within(0.01));
        }


        [Test]
        [TestCase(0, 0, 1)]
        [TestCase(0, 100, 0)]
        [TestCase(0, -1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(-1, 0, 0)]
        [TestCase(-1, 1, 0)]
        [TestCase(-1, 100, 0)]
        [TestCase(-1, -1, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 0.5)]
        [TestCase(1, 3, 0.33)]
        [TestCase(2, 3, 0.66)]
        [TestCase(0, 10, 0)]
        [TestCase(1, 10, 0.1)]
        [TestCase(2, 10, 0.2)]
        [TestCase(3, 10, 0.3)]
        [TestCase(4, 10, 0.4)]
        [TestCase(5, 10, 0.5)]
        [TestCase(6, 10, 0.6)]
        [TestCase(7, 10, 0.7)]
        [TestCase(8, 10, 0.8)]
        [TestCase(9, 10, 0.9)]
        [TestCase(10, 10, 1)]
        public void LinearEaserTests(int index, int total, double expectedValue)
        {
            Assert.That(LinearEaser.ProgressAt(index, total), Is.EqualTo(expectedValue).Within(0.01));
        }

        [Test]
        [TestCase(0, 0, 1)]
        [TestCase(0, 100, 0)]
        [TestCase(0, -1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(-1, 0, 0)]
        [TestCase(-1, 1, 0)]
        [TestCase(-1, 100, 0)]
        [TestCase(-1, -1, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 0.5)]
        [TestCase(1, 3, 0.25)]
        [TestCase(2, 3, 0.75)]
        [TestCase(0, 10, 0)]
        [TestCase(1, 10, 0.02)]
        [TestCase(2, 10, 0.09)]
        [TestCase(3, 10, 0.2)]
        [TestCase(4, 10, 0.34)]
        [TestCase(5, 10, 0.5)]
        [TestCase(6, 10, 0.65)]
        [TestCase(7, 10, 0.79)]
        [TestCase(8, 10, 0.90)]
        [TestCase(9, 10, 0.97)]
        [TestCase(10, 10, 1)]
        public void SineEaserTests(int index, int total, double expectedValue)
        {
            Assert.That(SineEaser.ProgressAt(index, total), Is.EqualTo(expectedValue).Within(0.01));
        }
    }
}