using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Data;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    public class StringTransformationTests
    {
        private TestRectangle _testRectangle;

        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }

        [Test]
        public void TransformStringProperty()
        {
            PropertyTransformation transformation =
                TaskPlexFactory.GetStringTransformation(_testRectangle, "Name", "Start", "End", 10, 1);
            transformation.StartAsync().Wait();
            Assert.That(_testRectangle.Name == "End");
        }
    }
}