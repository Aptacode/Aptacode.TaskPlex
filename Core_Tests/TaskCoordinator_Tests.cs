using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using Aptacode.Core;

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
            int destinationValue = 100;
            PropertyTransformation transformation = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(90));
            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<int> changeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };
            transposer.Start();

            transposer.Apply(transformation);

            List<int> expectedChangeLog = new List<int>() { 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog), Is.True.After(150, 150));
        }

        [Test]
        public void Parallel_Transformations()
        {
            PropertyTransformation transformation1 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 100;
                },
                TimeSpan.FromMilliseconds(90));

            PropertyTransformation transformation2 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Height"),
                () =>
                {
                    return 1000;
                },
                TimeSpan.FromMilliseconds(100));

            PropertyTransformation transformation3 = new DoubleInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Opacity"),
                () =>
                {
                    return 1;
                },
                TimeSpan.FromMilliseconds(100));


            transformation1.SteoDuration = TimeSpan.FromMilliseconds(10);
            transformation2.SteoDuration = TimeSpan.FromMilliseconds(10);
            transformation3.SteoDuration = TimeSpan.FromMilliseconds(20);

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

            List<int> expectedChangeLog1 = new List<int>() { 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            List<int> expectedChangeLog2 = new List<int>() { 118,216,314,412,510,608,706,804,902,1000 };
            List<double> expectedChangeLog3 = new List<double>() { 0.2,0.4,0.6,0.8,1 };
            Assert.That(() => changeLog1.SequenceEqual(expectedChangeLog1), Is.True.After(200, 200));
            Assert.That(() => changeLog2.SequenceEqual(expectedChangeLog2), Is.True.After(200, 200));
            Assert.That(() => changeLog3.SequenceEqual(expectedChangeLog3, new DoubleComparer()), Is.True.After(200, 200));
        }


        [Test]
        public void Colliding_Transformations()
        {
            PropertyTransformation transformation1 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 100;
                },
                TimeSpan.FromMilliseconds(90));

            PropertyTransformation transformation2 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 50;
                },
                TimeSpan.FromMilliseconds(50));


            transformation1.SteoDuration = TimeSpan.FromMilliseconds(10);
            transformation2.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<int> changeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transposer.Start();

            transposer.Apply(transformation1);
            transposer.Apply(transformation2);

            List<int> expectedChangeLog = new List<int>() { 20, 30, 40, 50, 60, 70, 80, 90, 100, 90, 80, 70, 60, 50 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog), Is.True.After(200, 200));
        }
    }
}