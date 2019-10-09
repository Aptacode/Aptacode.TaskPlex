using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskCoordinator.Tasks;
using TaskCoordinator.Tasks.Transformation;
using TaskCoordinator.Tasks.Transformation.Interpolaton;

namespace Aptacode_TaskCoordinator_Tests
{

    public class LinearGroupTask_Tests
    {
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        [Test]
        public void LinearTransformation()
        {
            PropertyTransformation transformation1 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 100;
                },
                TimeSpan.FromMilliseconds(90));
            transformation1.SteoDuration = TimeSpan.FromMilliseconds(10);

            PropertyTransformation transformation2 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 0;
                },
                TimeSpan.FromMilliseconds(100));
            transformation2.SteoDuration = TimeSpan.FromMilliseconds(10);

            PropertyTransformation transformation3 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 50;
                },
                TimeSpan.FromMilliseconds(50));
            transformation3.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<int> changeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                changeLog.Add(e.NewValue);
            };


            LinearGroupTask groupTask = new LinearGroupTask(new List<BaseTask>() { transformation1 , transformation2, transformation3});
            groupTask.Start();

            List<int> expectedChangeLog = new List<int>() { 20, 30, 40, 50, 60, 70, 80, 90, 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 0, 10, 20, 30, 40, 50 };
            Assert.That(() => changeLog.SequenceEqual(expectedChangeLog), Is.True.After(400, 400));
        }
    }
}
