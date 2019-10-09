using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using Aptacode.Core;
using Aptacode.TaskPlex.Core_Tests.Utilites;

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
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetIntInterpolation(testRectangle, "Width", 0, 100, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));

            List<int> changeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };
            transposer.Start();

            transposer.Apply(transformation);

            List<int> expectedChangeLog = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog), Is.True.After(150, 150));
        }

        [Test]
        public void Parallel_Transformations()
        {
            PropertyTransformation transformation1 = PropertyTransformation_Helpers.GetIntInterpolation(testRectangle, "Width", 0, 100, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));
            PropertyTransformation transformation2 = PropertyTransformation_Helpers.GetIntInterpolation(testRectangle, "Height", 50, 100, TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(10));
            PropertyTransformation transformation3 = PropertyTransformation_Helpers.GetDoubleInterpolation(testRectangle, "Opacity", 0, 1.0, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(20));


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

            transposer.Apply(transformation1);
            transposer.Apply(transformation2);
            transposer.Apply(transformation3);

            List<int> expectedChangeLog1 = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            List<int> expectedChangeLog2 = new List<int>() { 60, 70, 80, 90, 100 };
            List<double> expectedChangeLog3 = new List<double>() { 0.2,0.4,0.6,0.8,1 };
            Assert.That(() => changeLog1.SequenceEqual(expectedChangeLog1), Is.True.After(200, 200));
            Assert.That(() => changeLog2.SequenceEqual(expectedChangeLog2), Is.True.After(200, 200));
            Assert.That(() => changeLog3.SequenceEqual(expectedChangeLog3, new DoubleComparer()), Is.True.After(200, 200));
        }


        [Test]
        public void Colliding_Transformations()
        {
            PropertyTransformation transformation1 = PropertyTransformation_Helpers.GetIntInterpolation(testRectangle, "Width", 0, 100, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));
            PropertyTransformation transformation2 = PropertyTransformation_Helpers.GetIntInterpolation(testRectangle, "Width", 50, TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(10));

            List<int> changeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transposer.Start();

            transposer.Apply(transformation1);
            transposer.Apply(transformation2);

            List<int> expectedChangeLog = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 90, 80, 70, 60, 50 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog), Is.True.After(200, 200));
        }
    }
}