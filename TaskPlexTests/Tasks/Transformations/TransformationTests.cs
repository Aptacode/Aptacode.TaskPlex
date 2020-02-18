using System;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Tasks.Transformations
{
    [TestFixture]
    public class TransformationTests
    {
        [Test]
        public void IntTransformationTests()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var intTransformation = new IntTransformation<TestRectangle>(testRectangle, "Width", () => 100,
                TimeSpan.FromMilliseconds(10), RefreshRate.Highest);

            //Act
            intTransformation.StartAsync().Wait();

            //Assert
            Assert.Inconclusive("Test is missing main body and asserts. Needs to be completed.");
        }
    }
}