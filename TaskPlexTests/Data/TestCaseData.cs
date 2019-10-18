using System.Collections.Generic;
using System.Drawing;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tests.Utilites;

namespace Aptacode.TaskPlex.Tests.Data
{
    public static class TaskPlexTestData
    {
        public static object[] GetNormalTaskExamples()
        {
            var testRectangle = new TestRectangle();
            return new object[]
            {
                new object[] {TaskPlexFactory.GetWaitTask(1)},
                new[] {TaskPlexFactory.GetIntInterpolator(10, 25, 20, 1)},
                new[] {TaskPlexFactory.GetDoubleInterpolator(10, 25.5, 20, 1)},
                new object[]
                {
                    TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", Color.Aqua, Color.Aqua, 20,
                        1)
                },
                new object[] {TaskPlexFactory.GetDoubleTransformation(testRectangle, "Opacity", 10, 25.5, 20, 1)},
                new object[] {TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 10, 25, 20, 1)},
                new object[]
                {
                    TaskPlexFactory.GetSequentialGroup(new List<BaseTask>
                        {TaskPlexFactory.GetWaitTask(20), TaskPlexFactory.GetWaitTask(20)})
                },
                new object[]
                {
                    TaskPlexFactory.GetParallelGroup(new List<BaseTask>
                        {TaskPlexFactory.GetWaitTask(20), TaskPlexFactory.GetWaitTask(20)})
                }
            };
        }

        public static object[] GetParallelTasks()
        {
            return new object[]
            {
                new object[] {new List<BaseTask> {TaskPlexFactory.GetWaitTask(1)}},
                new object[] {new List<BaseTask> {TaskPlexFactory.GetWaitTask(1), TaskPlexFactory.GetWaitTask(1)}}
            };
        }

        public static object[] GetCollidingTasks()
        {
            var testRectangle = new TestRectangle();
            return new object[]
            {
                new object[]
                {
                    new List<BaseTask>
                    {
                        TaskPlexFactory.GetSequentialGroup(new List<BaseTask>
                        {
                            TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1),
                            TaskPlexFactory.GetIntTransformation(testRectangle, "Height", 100, 0, 10, 1)
                        }),
                        TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1)
                    }
                },

                new object[]
                {
                    new List<BaseTask>
                    {
                        TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1),
                        TaskPlexFactory.GetSequentialGroup(new List<BaseTask>
                        {
                            TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1),
                            TaskPlexFactory.GetIntTransformation(testRectangle, "Height", 100, 0, 10, 1)
                        })
                    }
                }
            };
        }

        public static object[] GetNonZeroTransformationAndInterval()
        {
            var testRectangle = new TestRectangle();
            return new object[]
            {
                new object[]
                {
                    TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", Color.Aqua,
                        Color.FromArgb(100, 200, 20, 30), 2, 1),
                    Color.FromArgb(100, 200, 20, 30)
                },
                new object[] {TaskPlexFactory.GetDoubleTransformation(testRectangle, "Opacity", 10, 25.5, 2, 1), 25.5},
                new object[] {TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 10, 25, 2, 1), 25},
                new object[]
                    {TaskPlexFactory.GetStringTransformation(testRectangle, "Name", "Start", "End", 2, 1), "End"}
            };
        }

        public static object[] GetInstantTransformations()
        {
            var testRectangle = new TestRectangle();
            return new object[]
            {
                new object[]
                {
                    TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", Color.Aqua,
                        Color.FromArgb(100, 200, 20, 30), 0, 1),
                    Color.FromArgb(100, 200, 20, 30)
                },
                new object[] {TaskPlexFactory.GetDoubleTransformation(testRectangle, "Opacity", 10, 25.5, 0, 1), 25.5},
                new object[] {TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 10, 25, 0, 1), 25},
                new object[]
                    {TaskPlexFactory.GetStringTransformation(testRectangle, "Name", "Start", "End", 0, 1), "End"}
            };
        }

        public static object[] GetZeroIntervalTransformations()
        {
            var testRectangle = new TestRectangle();
            return new object[]
            {
                new object[]
                {
                    TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", Color.Aqua,
                        Color.FromArgb(100, 200, 20, 30), 2, 0),
                    Color.FromArgb(100, 200, 20, 30)
                },
                new object[] {TaskPlexFactory.GetDoubleTransformation(testRectangle, "Opacity", 10, 25.5, 2, 0), 25.5},
                new object[] {TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 10, 25, 2, 0), 25},
                new object[]
                    {TaskPlexFactory.GetStringTransformation(testRectangle, "Name", "Start", "End", 2, 0), "End"}
            };
        }

        public static object[] GetColorInterpolationData()
        {
            return new object[]
            {
                new object[]
                {
                    Color.FromArgb(0, 0, 0, 0), Color.FromArgb(30, 30, 30, 30),
                    new List<Color>
                        {Color.FromArgb(10, 10, 10, 10), Color.FromArgb(20, 20, 20, 20), Color.FromArgb(30, 30, 30, 30)}
                },
                new object[]
                {
                    Color.FromArgb(30, 30, 30, 30), Color.FromArgb(0, 0, 0, 0),
                    new List<Color>
                        {Color.FromArgb(20, 20, 20, 20), Color.FromArgb(10, 10, 10, 10), Color.FromArgb(0, 0, 0, 0)}
                },
                new object[]
                {
                    Color.FromArgb(255, 0, 30, 10), Color.FromArgb(255, 30, 0, 10),
                    new List<Color>
                    {
                        Color.FromArgb(255, 10, 20, 10), Color.FromArgb(255, 20, 10, 10), Color.FromArgb(255, 30, 0, 10)
                    }
                }
            };
        }

        public static object[] GetDoubleInterpolationData()
        {
            return new object[]
            {
                new object[] {0, 1, new List<double> {0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0}},
                new object[] {0, -1, new List<double> {-0.1, -0.2, -0.3, -0.4, -0.5, -0.6, -0.7, -0.8, -0.9, -1.0}},
                new object[] {1, 1, new List<double> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}}
            };
        }

        public static object[] GetIntInterpolationData()
        {
            return new object[]
            {
                new object[] {0, 100, new List<int> {10, 20, 30, 40, 50, 60, 70, 80, 90, 100}},
                new object[] {0, -100, new List<int> {-10, -20, -30, -40, -50, -60, -70, -80, -90, -100}},
                new object[] {1, 1, new List<int> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}}
            };
        }

        public static object[] GetLinearEaserData()
        {
            return new object[]
            {
                new object[] {0, 100, new List<int> {10, 20, 30, 40, 50, 60, 70, 80, 90, 100}, new LinearEaser()},
                new object[]
                    {0, -100, new List<int> {-10, -20, -30, -40, -50, -60, -70, -80, -90, -100}, new LinearEaser()},
                new object[] {1, 1, new List<int> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, new LinearEaser()}
            };
        }

        public static object[] GetEaserData()
        {
            return new object[]
            {
                new object[]
                    {new CubicInEaser(), new List<double> {0, 0.01, 0.04, 0.09, 0.16, 0.25, 0.36, 0.49, 0.64, 0.81, 1}},
                new object[]
                {
                    new CubicOutEaser(),
                    new List<double> {0, 0.31622, 0.44721, 0.54772, 0.6324, 0.7071, 0.77454, 0.8366, 0.8944, 0.9486, 1}
                }
            };
        }

        public static object[] GetExpectedEaserData()
        {
            return new object[]
            {
                new object[] {0, 1, new List<double> {0, 0, 0, 0.1, 0.2, 0.3, 0.4, 0.6, 0.8, 1.0}, new CubicInEaser()},
                new object[]
                    {0, -1, new List<double> {0, 0, 0, -0.1, -0.2, -0.3, -0.4, -0.6, -0.8, -1.0}, new CubicInEaser()},
                new object[] {1, 1, new List<double> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, new CubicInEaser()},
                new object[]
                    {0, 1, new List<double> {0.3, 0.4, 0.5, 0.6, 0.7, 0.7, 0.8, 0.8, 0.9, 1.0}, new CubicOutEaser()},
                new object[]
                {
                    0, -1, new List<double> {-0.3, -0.4, -0.5, -0.6, -0.7, -0.7, -0.8, -0.8, -0.9, -1.0},
                    new CubicOutEaser()
                },
                new object[] {1, 1, new List<double> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, new CubicOutEaser()},
                new object[]
                    {0, 1, new List<double> {0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0}, new LinearEaser()},
                new object[]
                {
                    0, -1, new List<double> {-0.1, -0.2, -0.3, -0.4, -0.5, -0.6, -0.7, -0.8, -0.9, -1.0},
                    new LinearEaser()
                },
                new object[] {1, 1, new List<double> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, new LinearEaser()}
            };
        }

        public static object[] Template()
        {
            var testRectangle = new TestRectangle();
            return new object[]
            {
            };
        }
    }
}