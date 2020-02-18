using System;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Tasks.Transformations
{
    [TestFixture]
    public class TransformationTests
    {
        [Test]
        public void IntTransformationConstructorTests()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var transformation = TransformationFactory.Create(testRectangle, "Width", 100, TimeSpan.FromMilliseconds(10), RefreshRate.Highest);

            //Assert
            Assert.AreEqual(testRectangle, transformation.Target);
            Assert.AreEqual("Width", transformation.Property);
            Assert.AreEqual(TimeSpan.FromMilliseconds(10), transformation.Duration);
            Assert.AreEqual(TaskState.Ready, transformation.State);
        }
        [Test]
        public void DoubleTransformationConstructorTests()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var transformation = TransformationFactory.Create(testRectangle, "Opacity", 10.5, TimeSpan.FromMilliseconds(10), RefreshRate.Highest);

            //Assert
            Assert.AreEqual(testRectangle, transformation.Target);
            Assert.AreEqual("Opacity", transformation.Property);
            Assert.AreEqual(TimeSpan.FromMilliseconds(10), transformation.Duration);
            Assert.AreEqual(TaskState.Ready, transformation.State);

        }

    }
}