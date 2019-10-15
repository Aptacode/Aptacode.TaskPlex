using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Transformations
{
    public class TransformationTests
    {
        TestRectangle _testRectangle;

        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }

        [Test]
        public void StartAndFinishEvents()
        {
            PropertyTransformation transformation = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 100, 10, 1);

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

            transformation.StartAsync().Wait();


            Assert.That(startedCalled && finishedCalled);
        }

        [Test]
        public void ZeroTransformationDuration()
        {
            PropertyTransformation transformation = PropertyTransformationHelpers.GetDoubleTransformation(_testRectangle, "Opacity", 0, 1, 0, 1);

            List<double> changeLog = new List<double>();
            _testRectangle.OnOpacityChanged += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };

            transformation.StartAsync().Wait();

            List<double> expectedChangeLog = new List<double>() { 1.0 };
            Assert.That(changeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()));
        }


        [Test]
        public void ZeroIntervalDuration()
        {
            PropertyTransformation transformation = PropertyTransformationHelpers.GetDoubleTransformation(_testRectangle, "Opacity", 0, 1, 1, 0);

            transformation.StartAsync().Wait();

            Assert.That(new DoubleComparer().Equals(_testRectangle.Opacity, 1.0));
        }
    }
}