using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Tasks.Transformations.Interpolator
{
    [TestFixture]
    public class InterpolatorTests
    {
        private static readonly IntInterpolator IntInterpolator = new IntInterpolator();
        private static readonly DoubleInterpolator DoubleInterpolator = new DoubleInterpolator();

        [Test]
        [TestCase(0, 0, 0, new double[] { })]
        [TestCase(0, 1, 0, new double[] { })]
        [TestCase(1, 0, 0, new double[] { })]
        [TestCase(0, 0, 1, new double[] {0})]
        [TestCase(0, 1, 1, new double[] {1})]
        [TestCase(1, 0, 1, new double[] {0})]
        [TestCase(0, 0, 2, new double[] {0, 0})]
        [TestCase(0, 1, 2, new[] {0.5, 1})]
        [TestCase(1, 0, 2, new[] {0.5, 0})]
        [TestCase(0, 1, 10, new[] {0.1, 0.2, 0.3, 0.40, 0.50, 0.60, 0.70, 0.80, 0.90, 1})]
        [TestCase(1, 0, 10, new[] {0.90, 0.80, 0.70, 0.60, 0.50, 0.40, 0.30, 0.20, 0.10, 0})]
        [TestCase(0, -1, 10, new[] {-0.1, -0.2, -0.3, -0.40, -0.50, -0.60, -0.70, -0.80, -0.90, -1})]
        public void DoubleInterpolationTests(int startValue, int endValue, int stepCount, double[] steps)
        {
            var result = DoubleInterpolator.Interpolate(startValue, endValue, stepCount, Easers.Linear);
            Assert.That(result.Count(), Is.EqualTo(steps.Length));
            for (var i = 0; i < result.Count(); i++)
            {
                Assert.That(result.ElementAt(i), Is.EqualTo(steps[i]).Within(0.01));
            }
        }

        [Test]
        [TestCase(0, 0, 0, new int[] { })]
        [TestCase(0, 1, 0, new int[] { })]
        [TestCase(1, 0, 0, new int[] { })]
        [TestCase(0, 0, 1, new[] {0})]
        [TestCase(0, 1, 1, new[] {1})]
        [TestCase(1, 0, 1, new[] {0})]
        [TestCase(0, 0, 2, new[] {0, 0})]
        [TestCase(0, 1, 2, new[] {0, 1})]
        [TestCase(1, 0, 2, new[] {1, 0})]
        [TestCase(0, 100, 10, new[] {10, 20, 30, 40, 50, 60, 70, 80, 90, 100})]
        [TestCase(100, 0, 10, new[] {90, 80, 70, 60, 50, 40, 30, 20, 10, 0})]
        [TestCase(0, -10, 10, new[] {-1, -2, -3, -4, -5, -6, -7, -8, -9, -10})]
        public void IntInterpolationTests(int startValue, int endValue, int stepCount, int[] steps)
        {
            Assert.That(IntInterpolator.Interpolate(startValue, endValue, stepCount, Easers.Linear),
                Is.EquivalentTo(steps));
        }
    }
}