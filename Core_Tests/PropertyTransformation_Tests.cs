using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.Core_Tests.Utilites;
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
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetIntInterpolation(testRectangle, "Width", 0, 100, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));

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
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetDoubleInterpolation(testRectangle, "Opacity", 0, 1, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(10));

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
            PropertyTransformation transformation = PropertyTransformation_Helpers.GetDoubleInterpolation(testRectangle, "Opacity", 0, 1, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(0));

            List<double> changeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transformation.Start();
            Assert.That(() => changeLog.Count == 100 && new DoubleComparer().Equals(testRectangle.Opacity, 1.0), Is.True.After(150, 150));
        }
    }
}