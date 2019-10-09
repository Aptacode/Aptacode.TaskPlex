using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.TaskPlex.Core_Tests
{
    public class Transformation_Tests
    {
        PropertyTransformation transformation;
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        [Test]
        public void StartAndFinishEvents()
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


            bool startedCalled = false;
            bool finishedCalled = false;

            transformation.OnStarted += (s, e) =>
            {
                startedCalled = true;
            };

            transformation.OnFinished += (s, e) =>
            {
                finishedCalled = true;
            };

            transformation.Start();

            Assert.That(() => startedCalled == true, Is.True.After(200, 200), "OnStarted was not fired");
            Assert.That(() => finishedCalled == true, Is.True.After(200, 200), "OnFinished was not fired");
        }

        [Test]
        public void ZeroTransformationDuration()
        {
            double destinationValue = 1;
            transformation = new DoubleInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Opacity"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(0));

            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<double> changeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transformation.Start();

            List<double> expectedChangeLog = new List<double>() { 1.0 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()), Is.True.After(150, 150));
        }


        [Test]
        public void ZeroIntervalDuration()
        {
            double destinationValue = 1;
            transformation = new DoubleInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Opacity"),
                () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(10));

            transformation.SteoDuration = TimeSpan.FromMilliseconds(0);

            List<double> changeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transformation.Start();

            List<double> expectedChangeLog = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()), Is.True.After(150, 150));
        }

        [Test]
        public void DestinationValueFunction()
        {
            double destinationValue = 1;
            transformation = new DoubleInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Opacity"), () =>
                {
                    return destinationValue;
                },
                TimeSpan.FromMilliseconds(10));

            transformation.SteoDuration = TimeSpan.FromMilliseconds(0);

            List<double> changeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transformation.Start();

            List<double> expectedChangeLog = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()), Is.True.After(150, 150));
        }

        [Test]
        public void DestinationValueConstant()
        {
            double destinationValue = 1;
            transformation = new DoubleInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Opacity"),1,
                TimeSpan.FromMilliseconds(10));

            transformation.SteoDuration = TimeSpan.FromMilliseconds(0);

            List<double> changeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transformation.Start();

            List<double> expectedChangeLog = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()), Is.True.After(150, 150));
        }

    }
}