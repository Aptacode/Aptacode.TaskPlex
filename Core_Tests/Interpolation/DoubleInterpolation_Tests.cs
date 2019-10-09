﻿using Aptacode_TaskCoordinator.Tests.Utilites;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Core.Tasks.Transformations;
using Aptacode.Core.Tasks.Transformations.Interpolation;

namespace Aptacode.TaskPlex.Core_Tests
{

    public class DoubleInterpolation_Tests
    {
        PropertyTransformation transformation;
        TestRectangle testRectangle;

        [SetUp]
        public void Setup()
        {
            testRectangle = new TestRectangle();
        }

        private void Interpolation_Expected_Change_Log(double startValue, double endValue, List<double> expectedChangeLog)
        {
            testRectangle.Opacity = startValue;

            transformation = new DoubleInterpolation(
                testRectangle,
                testRectangle.GetType().GetProperty("Opacity"),
                () =>
                {
                    return endValue;
                },
                TimeSpan.FromMilliseconds(100));

            transformation.SteoDuration = TimeSpan.FromMilliseconds(10);

            List<double> actualChangeLog = new List<double>();
            testRectangle.OnOpacityChanged += (s, e) =>
            {
                actualChangeLog.Add(e.NewValue);
            };

            transformation.Start();

            Assert.That(() => actualChangeLog.SequenceEqual(expectedChangeLog, new DoubleComparer()), Is.True.After(120, 120));
        }

        [Test]
        public void DoubleInterpolation_PositiveChange()
        {
            Interpolation_Expected_Change_Log(0, 1, new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0 });
        }

        [Test]
        public void DoubleInterpolation_NegativeChange()
        {
            Interpolation_Expected_Change_Log(0, -1, new List<double>() { -0.1, -0.2, -0.3, -0.4, -0.5, -0.6, -0.7, -0.8, -0.9, -1.0 });
        }

        [Test]
        public void DoubleInterpolation_NoChange()
        {
            Interpolation_Expected_Change_Log(1, 1, new List<double>() { 1, 1, 1, 1, 1, 1 ,1, 1, 1, 1 });
        }
    }
}