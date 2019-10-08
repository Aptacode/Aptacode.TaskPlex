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

    public class ParallelGroupTask_Tests
    {
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        [Test]
        public void ParallelTransformation()
        {
            PropertyTransformation transformation1 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 100;
                },
                TimeSpan.FromMilliseconds(90));
            transformation1.Interval = TimeSpan.FromMilliseconds(10);

            PropertyTransformation transformation2 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 0;
                },
                TimeSpan.FromMilliseconds(100));
            transformation2.Interval = TimeSpan.FromMilliseconds(10);

            PropertyTransformation transformation3 = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return 50;
                },
                TimeSpan.FromMilliseconds(50));
            transformation3.Interval = TimeSpan.FromMilliseconds(10);

            bool firstToEnd = true;
            DateTime latestStartTime = DateTime.Now;
            DateTime earliestEndTime = DateTime.Now;

            transformation1.OnStarted += (s, e) =>
            {
                latestStartTime = DateTime.Now;
            };

            transformation1.OnFinished += (s, e) =>
            {
                if (firstToEnd)
                {
                    earliestEndTime = DateTime.Now;
                    firstToEnd = true;
                }
            };

            transformation2.OnStarted += (s, e) =>
            {
                latestStartTime = DateTime.Now;
            };

            transformation2.OnFinished += (s, e) =>
            {
                if (firstToEnd)
                {
                    earliestEndTime = DateTime.Now;
                    firstToEnd = true;
                }
            };

            transformation3.OnStarted += (s, e) =>
            {
                latestStartTime = DateTime.Now;
            };

            transformation3.OnFinished += (s, e) =>
            {
                if (firstToEnd)
                {
                    earliestEndTime = DateTime.Now;
                    firstToEnd = true;
                }
            };


            ParallelGroupTask groupTask = new ParallelGroupTask(new List<BaseTask>() { transformation1, transformation2, transformation3 });
            groupTask.Start();



            Assert.That(() => latestStartTime.CompareTo(earliestEndTime) < 0, Is.True.After(400, 400));
        }
    }
}
