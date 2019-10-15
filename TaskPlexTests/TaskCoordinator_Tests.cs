using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    public class PropertyTransformationTests
    {

        TaskCoordinator _taskCoordinator;
        TestRectangle _testRectangle;

        [SetUp]
        public void Setup()
        {
            _taskCoordinator = new TaskCoordinator();
            _testRectangle = new TestRectangle();
        }

        [Test]
        public void Single_Transformation()
        {
            PropertyTransformation transformation = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 100, 10, 1);

            List<int> changeLog = new List<int>();
            _testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            transformation.OnFinished += (s, e) =>
            {
                tcs.SetResult(true);
            };

            _taskCoordinator.Apply(transformation);

            tcs.Task.Wait();

            List<int> expectedChangeLog = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            Assert.That(changeLog.SequenceEqual(expectedChangeLog));
        }

        [Test]
        public void Parallel_Transformations()
        {
            PropertyTransformation transformation1 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 50, 5, 1);
            PropertyTransformation transformation2 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Height", 100, 0, 10, 1);
            PropertyTransformation transformation3 = PropertyTransformationHelpers.GetDoubleTransformation(_testRectangle, "Opacity", 0, 0.5, 5, 1);


            List<int> changeLog1 = new List<int>();
            List<int> changeLog2 = new List<int>();
            List<double> changeLog3 = new List<double>();
            _testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog1.Add(e.NewValue);
            };

            _testRectangle.OnHeightChange += (s, e) =>
            {
                changeLog2.Add(e.NewValue);
            };

            _testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog3.Add(e.NewValue);
            };

            List<int> expectedChangeLog1 = new List<int> { 10, 20, 30, 40 ,50 };
            List<int> expectedChangeLog2 = new List<int> { 90, 80, 70, 60, 50, 40, 30, 20, 10, 0 };
            List<double> expectedChangeLog3 = new List<double> { 0.1,0.2,0.3,0.4,0.5 };


            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            transformation2.OnFinished += (s, e) =>
            {
                tcs.SetResult(true);
            };

            _taskCoordinator.Apply(transformation1);
            _taskCoordinator.Apply(transformation2);
            _taskCoordinator.Apply(transformation3);

            tcs.Task.Wait();

            Assert.That(changeLog1.SequenceEqual(expectedChangeLog1) && changeLog2.SequenceEqual(expectedChangeLog2) && changeLog3.SequenceEqual(expectedChangeLog3, new DoubleComparer()));
        }


        [Test]
        public void Colliding_Transformations()
        {
            PropertyTransformation transformation1 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 100, 10, 1);
            PropertyTransformation transformation2 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 50, 5, 1);

            List<int> changeLog = new List<int>();
            _testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            transformation2.OnFinished += (s, e) =>
            {
                tcs.SetResult(true);
            };

            _taskCoordinator.Apply(transformation1);
            _taskCoordinator.Apply(transformation2);

            tcs.Task.Wait();

            List<int> expectedChangeLog = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 90, 80, 70, 60, 50 };
            Assert.That(changeLog.SequenceEqual(expectedChangeLog));
        }
    }
}