using System.Collections.Generic;
using System.Drawing;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tests.Utilites;

namespace Aptacode.TaskPlex.Tests.Data
{
    public class TaskPlexTestData
    {
        public static object[] GetNormalTaskExamples()
        {
            TestRectangle testRectangle = new TestRectangle();
            return new object[]
            {
                new object[] {TaskPlexFactory.GetWaitTask(1)},
                new object[] {TaskPlexFactory.GetIntInterpolator(10, 25, 20, 1)},
                new object[] {TaskPlexFactory.GetDoubleInterpolator(10, 25.5, 20, 1)},
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
                },
            };

        }      
        
        public static object[] GetParallelTasks()
        {
            TestRectangle testRectangle = new TestRectangle();
            return new object[]
            {
                new object[] { new List<BaseTask>{ TaskPlexFactory.GetWaitTask(1) }  },
                new object[] { new List<BaseTask>{ TaskPlexFactory.GetWaitTask(1), TaskPlexFactory.GetWaitTask(1) }  },
            };
        }

        public static object[] GetCollidingTasks()
        {
            TestRectangle testRectangle = new TestRectangle();
            return new object[]
            {
                new object[]{new List<BaseTask>
                {               
                    TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 0, 100, 10, 1),
                    TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 100, 0, 10, 1)
                }
                }

            };
        }

        public static object[] GetNonZeroTransformationAndInterval()
        {
            TestRectangle testRectangle = new TestRectangle();
            return new object[]
            {
                new object[] { TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", Color.Aqua, Color.FromArgb(100,200,20,30), 2,1),Color.FromArgb(100,200,20,30) },
                new object[] { TaskPlexFactory.GetDoubleTransformation(testRectangle, "Opacity", 10, 25.5, 2,1), 25.5  },
                new object[] { TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 10, 25, 2,1), 25  },
                new object[] { TaskPlexFactory.GetStringTransformation(testRectangle, "Name", "Start", "End", 2,1), "End"  },
            };
        }
        public static object[] GetInstantTransformations()
        {
            TestRectangle testRectangle = new TestRectangle();
            return new object[]
            {
                new object[] { TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", Color.Aqua, Color.FromArgb(100, 200, 20, 30), 0,1),Color.FromArgb(100,200,20,30)  },
                new object[] { TaskPlexFactory.GetDoubleTransformation(testRectangle, "Opacity", 10, 25.5, 0,1), 25.5  },
                new object[] { TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 10, 25, 0,1), 25  },
                new object[] { TaskPlexFactory.GetStringTransformation(testRectangle, "Name", "Start", "End", 0,1), "End"  },
            };
        }
        public static object[] GetZeroIntervalTransformations()
        {
            TestRectangle testRectangle = new TestRectangle();
            return new object[]
            {
                new object[] { TaskPlexFactory.GetColorTransformation(testRectangle, "BackgroundColor", Color.Aqua, Color.FromArgb(100, 200, 20, 30), 2,0), Color.FromArgb(100,200,20,30)  },
                new object[] { TaskPlexFactory.GetDoubleTransformation(testRectangle, "Opacity", 10, 25.5, 2,0), 25.5  },
                new object[] { TaskPlexFactory.GetIntTransformation(testRectangle, "Width", 10, 25, 2,0), 25  },
                new object[] { TaskPlexFactory.GetStringTransformation(testRectangle, "Name", "Start", "End", 2,0), "End"  },
            };
        }
        public static object[] Template()
        {
            TestRectangle testRectangle = new TestRectangle();
            return new object[]
            {

            };
        }
    }
}
