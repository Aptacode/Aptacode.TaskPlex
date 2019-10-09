using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;

namespace Aptacode.TaskPlex.Core_Tests
{

    public class Interpolation_Tests
    {
        PropertyTransformation transformation;
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        [Test]
        public void IntInterpolation_FinalValue()
        {
            int destinationValue = 100;
            transformation = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(100));

            transformation.Start();

            Assert.That(() => testRectangle.Width == destinationValue, Is.True.After(150, 150));
        }

        [Test]
        public void IntInterpolation_PositiveChange()
        {
            int destinationValue = 100;
            transformation = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(90));
            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<int> widthLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                widthLog.Add(e.NewValue);
            };

            transformation.Start();

            List<int> expectedWidthLog = new List<int>() { 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            Assert.That(() => widthLog.SequenceEqual(expectedWidthLog), Is.True.After(200, 200));
        }

        [Test]
        public void IntInterpolation_NegativeChange()
        {
            int destinationValue = -90;
            transformation = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(100));
            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<int> widthLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                widthLog.Add(e.NewValue);
            };

            transformation.Start();

            List<int> expectedWidthLog = new List<int>() { 0, -10, -20, -30, -40, -50, -60, -70, -80, -90 };
            Assert.That(() => widthLog.SequenceEqual(expectedWidthLog), Is.True.After(200, 200));
        }

        [Test]
        public void IntInterpolation_NoChange()
        {
            int destinationValue = 10;
            transformation = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(100));
            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<int> widthLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                widthLog.Add(e.NewValue);
            };

            transformation.Start();

            List<int> expectedWidthLog = new List<int>() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            Assert.That(() => widthLog.SequenceEqual(expectedWidthLog), Is.True.After(200, 200));
        }



        [Test]
        public void DoubleInterpolation_PositiveChange()
        {
            double destinationValue = 1;
            transformation = new DoubleInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Opacity"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(100));

            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<double> changeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transformation.Start();

            List<double> expectedChangeLog = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()), Is.True.After(200, 200));
        }
    }
}
