using System;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests.Tasks.Transformations
{
    [TestFixture]
    public class TransformationTests
    {
        [Test]
        public void CanPauseTasks()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var intTransformation = TaskPlexFactory.Create(testRectangle, "Width", 100,
                TimeSpan.FromMilliseconds(20), RefreshRate.High);

            var startTime = DateTime.Now;
            var endTime = DateTime.Now;

            intTransformation.OnStarted += (s, e) => { startTime = DateTime.Now; };
            intTransformation.OnFinished += (s, e) => { endTime = DateTime.Now; };

            //Action
            var task = Task.Run(async () =>
            {
                _ = intTransformation.StartAsync(new CancellationTokenSource()).ConfigureAwait(false);
                intTransformation.Pause();
                await Task.Delay(55);
                intTransformation.Resume();
                await Task.Delay(55);
            });

            task.Wait();

            //Assert
            Assert.Greater((endTime - startTime).Milliseconds, 70,
                "The transformation should finish over 100 ms after starting due to the delay");
        }

        [Test]
        public void CanPauseStringTransformation()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var transformation = TaskPlexFactory.Create(testRectangle, "Name", "New Name",
                TimeSpan.FromMilliseconds(20), RefreshRate.High);

            var startTime = DateTime.Now;
            var endTime = DateTime.Now;

            transformation.OnStarted += (s, e) => { startTime = DateTime.Now; };
            transformation.OnFinished += (s, e) => { endTime = DateTime.Now; };

            //Action
            var task = Task.Run(async () =>
            {
                _ = transformation.StartAsync(new CancellationTokenSource()).ConfigureAwait(false);

                transformation.Pause();
                await Task.Delay(55);
                transformation.Resume();
                await Task.Delay(55);
            });

            task.Wait();

            //Assert
            Assert.Greater((endTime - startTime).Milliseconds, 70,
                "The transformation should finish over 100 ms after starting due to the delay");
        }


        [Test]
        public void DoubleTransformationConstructorTests()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var transformation = TaskPlexFactory.Create(testRectangle, "Opacity", 10.5,
                TimeSpan.FromMilliseconds(10), RefreshRate.High);

            //Assert
            Assert.AreEqual(testRectangle, transformation.Target);
            Assert.AreEqual("Opacity", transformation.Property);
            Assert.AreEqual(TimeSpan.FromMilliseconds(10), transformation.Duration);
            Assert.AreEqual(TaskState.Ready, transformation.State);
        }

        [Test]
        public void IntTransformationConstructorTests()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var transformation = TaskPlexFactory.Create(testRectangle, "Width", 100,
                TimeSpan.FromMilliseconds(10), RefreshRate.High);

            //Assert
            Assert.AreEqual(testRectangle, transformation.Target);
            Assert.AreEqual("Width", transformation.Property);
            Assert.AreEqual(TimeSpan.FromMilliseconds(10), transformation.Duration);
            Assert.AreEqual(TaskState.Ready, transformation.State);
        }

        [Test]
        public void TransformationStartFinishEvents()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var transformation = TaskPlexFactory.Create(testRectangle, "Width", 100,
                TimeSpan.FromMilliseconds(10), RefreshRate.High);

            var hasStarted = false;
            var hasFinished = false;
            transformation.OnStarted += (s, e) => { hasStarted = true; };
            transformation.OnFinished += (s, e) => { hasFinished = true; };

            //Action
            var task = Task.Run(async () =>
                await transformation.StartAsync(new CancellationTokenSource()).ConfigureAwait(false));
            task.Wait();

            //Assert
            Assert.IsTrue(hasStarted, "OnStarted should be fired");
            Assert.IsTrue(hasFinished, "OnFinished should be fired");
        }


        [Test]
        public void WaitTaskStartFinishEvents()
        {
            //Arrange
            var testRectangle = new TestRectangle();
            var transformation = TaskPlexFactory.Wait(TimeSpan.FromMilliseconds(20));

            var hasStarted = false;
            var hasFinished = false;
            transformation.OnStarted += (s, e) => { hasStarted = true; };
            transformation.OnFinished += (s, e) => { hasFinished = true; };

            //Action
            var task = Task.Run(async () =>
                await transformation.StartAsync(new CancellationTokenSource()).ConfigureAwait(false));
            task.Wait();

            //Assert
            Assert.IsTrue(hasStarted, "OnStarted should be fired");
            Assert.IsTrue(hasFinished, "OnFinished should be fired");
        }

    }
}