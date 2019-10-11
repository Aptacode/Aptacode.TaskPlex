using Aptacode.Core.Tasks;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;
using Aptacode.TaskPlex.Core_Tests.Utilites;
using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aptacode.TaskPlex.Core_Tests
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
            PropertyTransformation transformation1 = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1);
            PropertyTransformation transformation2 = PropertyTransformation_Helpers.GetIntTransformation(testRectangle, "Width", 0, 10, 1);

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


            ParallelGroupTask groupTask = new ParallelGroupTask(new List<BaseTask>() { transformation1, transformation2});
            groupTask.StartAsync().Wait();

            Assert.That(latestStartTime.CompareTo(earliestEndTime) < 0);
        }
    }
}
