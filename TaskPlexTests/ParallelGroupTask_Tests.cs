using System;
using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.Tests.Utilites;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{

    public class ParallelGroupTaskTests
    {
        private TestRectangle _testRectangle;

        [SetUp]
        public void Setup()
        {
            _testRectangle = new TestRectangle();
        }

        [Test]
        public void ParallelTransformation()
        {
            PropertyTransformation transformation1 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 100, 10, 1);
            PropertyTransformation transformation2 = PropertyTransformationHelpers.GetIntTransformation(_testRectangle, "Width", 0, 10, 1);

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


            ParallelGroupTask groupTask = new ParallelGroupTask(new List<BaseTask> { transformation1, transformation2});
            groupTask.StartAsync().Wait();

            Assert.That(latestStartTime.CompareTo(earliestEndTime) < 0);
        }
    }
}
