using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{

    public class LinearGroupTaskTests
    {
        private TestRectangle _testRectangle;

        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }


        [Test]
        public void LinearGroup_SameProperty()
        {
            PropertyTransformation transformation1 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 100, 10, 1);
            PropertyTransformation transformation2 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 10, 1);
            PropertyTransformation transformation3 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 10, 10, 1);

            List<int> changeLog = new List<int>();
            _testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            LinearGroupTask groupTask = new LinearGroupTask(new List<BaseTask>() { transformation1 , transformation2, transformation3 });
            groupTask.StartAsync().Wait();

            List<int> expectedChangeLog = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            Assert.That(changeLog.SequenceEqual(expectedChangeLog));
        }
    }
}
