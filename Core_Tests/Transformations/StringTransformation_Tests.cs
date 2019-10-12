using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.TaskPlex.Core_Tests.Utilites;

namespace Aptacode.TaskPlex.Core_Tests
{

    public class StringTransformation_Tests
    {
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        [Test]
        public void StringTransformationTest()
        {
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetStringTransformation(testRectangle, "Name", "Start", "End", 10, 1);

            transformation.StartAsync().Wait();

            Assert.That(testRectangle.Name == "End");
        }
    }
}
