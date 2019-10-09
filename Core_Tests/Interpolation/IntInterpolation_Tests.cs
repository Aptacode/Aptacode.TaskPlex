using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;

namespace Aptacode.TaskPlex.Core_Tests
{

    public class IntInterpolation_Tests
    {
        PropertyTransformation transformation;
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        private void Interpolation_Expected_Change_Log(int startValue, int endValue, List<int> expectedChangeLog)
        {
            testRectangle.Width = startValue;

            transformation = new IntInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Width"),
                () =>
                {
                    return endValue;
                },
                TimeSpan.FromMilliseconds(100));

            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<int> actualChangeLog = new List<int>();
            testRectangle.OnWidthChange += (s, e) =>
            {
                actualChangeLog.Add(e.NewValue);
            };

            transformation.Start();

            Assert.That(() => actualChangeLog.SequenceEqual(expectedChangeLog), Is.True.After(120, 120));
        }

        [Test]
        public void IntInterpolation_PositiveChange()
        {
            Interpolation_Expected_Change_Log(0, 100, new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100});
        }

        [Test]
        public void IntInterpolation_NegativeChange()
        {
            Interpolation_Expected_Change_Log(0, -100, new List<int>() { -10, -20, -30, -40, -50, -60, -70, -80, -90, -100 });
        }

        [Test]
        public void IntInterpolation_NoChange()
        {
            Interpolation_Expected_Change_Log(10, 10, new List<int>() { 10,10,10,10,10,10,10,10,10,10 });
        }
    }
}
