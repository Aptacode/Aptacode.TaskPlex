using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core;
using Aptacode.TaskPlex.Core_Tests.Utilites;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Core_Tests
{
    public class PropertyTransposer_Tests
    {

        TaskCoordinator transposer;
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            transposer = new TaskCoordinator();
            testRectangle = new TestRectangle();
        }

        [Test]
        public void Single_Transformation()
        {
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1);

            List<int> changeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transposer.Start();
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            transformation.OnFinished += (s, e) =>
            {
                tcs.SetResult(true);
            };

            transposer.Apply(transformation);

            tcs.Task.Wait();

            List<int> expectedChangeLog = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            Assert.That(changeLog.SequenceEqual(expectedChangeLog));
        }

        [Test]
        public void Parallel_Transformations()
        {
            PropertyTransformation transformation1 = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Width", 0, 50, 5, 1);
            PropertyTransformation transformation2 = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Height", 100, 0, 10, 1);
            PropertyTransformation transformation3 = PropertyTransformation_Helpers.GetDoubleTransformation(testRectangle, "Opacity", 0, 0.5, 5, 1);


            List<int> changeLog1 = new List<int>();
            List<int> changeLog2 = new List<int>();
            List<double> changeLog3 = new List<double>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog1.Add(e.NewValue);
            };

            testRectangle.OnHeigtChange += (s, e) =>
            {
                changeLog2.Add(e.NewValue);
            };

            testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog3.Add(e.NewValue);
            };

            transposer.Start();


            List<int> expectedChangeLog1 = new List<int> { 10, 20, 30, 40 ,50 };
            List<int> expectedChangeLog2 = new List<int> { 90, 80, 70, 60, 50, 40, 30, 20, 10, 0 };
            List<double> expectedChangeLog3 = new List<double> { 0.1,0.2,0.3,0.4,0.5 };


            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            transformation2.OnFinished += (s, e) =>
            {
                tcs.SetResult(true);
            };

            transposer.Apply(transformation1);
            transposer.Apply(transformation2);
            transposer.Apply(transformation3);

            tcs.Task.Wait();

            Assert.That(changeLog1.SequenceEqual(expectedChangeLog1) && changeLog2.SequenceEqual(expectedChangeLog2) && changeLog3.SequenceEqual(expectedChangeLog3, new DoubleComparer()));
        }


        [Test]
        public void Colliding_Transformations()
        {
            PropertyTransformation transformation1 = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1);
            PropertyTransformation transformation2 = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Width", 50, 5, 1);

            List<int> changeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transposer.Start();

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            transformation2.OnFinished += (s, e) =>
            {
                tcs.SetResult(true);
            };

            transposer.Apply(transformation1);
            transposer.Apply(transformation2);

            tcs.Task.Wait();

            List<int> expectedChangeLog = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 90, 80, 70, 60, 50 };
            Assert.That(changeLog.SequenceEqual(expectedChangeLog));
        }
    }
}