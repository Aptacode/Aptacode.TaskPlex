using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
    public class DummyTask : BaseTask
    {
        private int _tickCount;

        public DummyTask(TimeSpan duration) : base(duration)
        {
        }

        protected override async Task InternalTask()
        {
            _tickCount = 0;

            while (_tickCount < _stepCount && !CancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(1).ConfigureAwait(false);
            }
        }

        public override void Update()
        {
            if (!IsRunning())
            {
                return;
            }

            _tickCount++;
        }
    }

    [TestFixture]
    public class TaskCoordinatorTests
    {
        [SetUp]
        public void Setup()
        {
            _taskCoordinator = new TaskCoordinator(new NullLoggerFactory(), RefreshRate.High);
            _taskLog = new Dictionary<BaseTask, (DateTime, DateTime, DateTime)>();
            _taskCoordinator.Start();
        }

        private TaskCoordinator _taskCoordinator;
        private Dictionary<BaseTask, (DateTime, DateTime, DateTime)> _taskLog;

        private double Duration(BaseTask task)
        {
            var startEndTime = _taskLog[task];
            return (startEndTime.Item2 - startEndTime.Item1).TotalMilliseconds;
        }

        private void LogStartTime(object sender, EventArgs e)
        {
            var task = sender as BaseTask;
            _taskLog[task] = (DateTime.Now, _taskLog[task].Item2, _taskLog[task].Item3);
        }

        private void LogCancelTime(object sender, EventArgs e)
        {
            var task = sender as BaseTask;
            _taskLog[task] = (_taskLog[task].Item1, _taskLog[task].Item2, DateTime.Now);
        }

        private void LogEndTime(object sender, EventArgs e)
        {
            var task = sender as BaseTask;
            _taskLog[task] = (_taskLog[task].Item1, DateTime.Now, _taskLog[task].Item2);
        }

        private void LogTaskStartEnd(BaseTask task)
        {
            _taskLog.Add(task, (DateTime.MinValue, DateTime.MinValue, DateTime.MinValue));
            task.OnStarted += LogStartTime;
            task.OnCancelled += LogCancelTime;
            task.OnFinished += LogEndTime;
        }

        private bool StartsBefore(BaseTask a, BaseTask b)
        {
            return _taskLog[a].Item1 < _taskLog[b].Item1;
        }

        private bool EndsBefore(BaseTask a, BaseTask b)
        {
            return _taskLog[a].Item2 < _taskLog[b].Item2;
        }

        private bool WasCanceled(BaseTask a)
        {
            return _taskLog[a].Item3 > _taskLog[a].Item1;
        }


        [Test]
        public void CanApplyTask()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            LogTaskStartEnd(task1);

            var task = Task.Run(async () => { await _taskCoordinator.Apply(task1); });

            task.Wait();

            //Assert
            Assert.Greater(Duration(task1), 10,
                "The transformation should finish over 100 ms after starting due to the delay");
        }


        [Test]
        public void CanPauseAllTasks()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));
            LogTaskStartEnd(task1);
            LogTaskStartEnd(task2);

            var task = Task.Run(async () =>
            {
                _taskCoordinator.Apply(task1);
                _taskCoordinator.Apply(task2);

                _taskCoordinator.Pause();
                Task.Delay(30).Wait();
                _taskCoordinator.Resume();
                Task.Delay(50).Wait();
            });

            task.Wait();

            //Assert
            Assert.Greater(Duration(task1), 40,
                "The transformation should finish over 100 ms after starting due to the delay");
            Assert.Greater(Duration(task2), 40,
                "The transformation should finish over 100 ms after starting due to the delay");
        }

        [Test]
        public void CanPauseSpecificTasks()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));

            LogTaskStartEnd(task1);
            LogTaskStartEnd(task2);

            var task = Task.Run(async () =>
            {
                _taskCoordinator.Apply(task1);
                _taskCoordinator.Apply(task2);

                _taskCoordinator.Pause(task1);
                Task.Delay(30).Wait();
                _taskCoordinator.Resume(task1);
                Task.Delay(50).Wait();
            });

            task.Wait();

            //Assert
            Assert.Greater(Duration(task1), Duration(task2));
        }

        [Test]
        public void CanReset()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));

            LogTaskStartEnd(task1);
            LogTaskStartEnd(task2);

            var task = Task.Run(async () =>
            {
                _taskCoordinator.Apply(task1);
                _taskCoordinator.Reset();
                _taskCoordinator.Apply(task2);

                Task.Delay(50).Wait();
            });

            task.Wait();

            //Assert
            Assert.IsTrue(WasCanceled(task1));
            Assert.IsFalse(WasCanceled(task2));
        }

        [Test]
        public void CanStopAllTasks()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));

            LogTaskStartEnd(task1);
            LogTaskStartEnd(task2);

            var task = Task.Run(async () =>
            {
                _taskCoordinator.Apply(task1);
                _taskCoordinator.Apply(task2);

                _taskCoordinator.Stop();

                Task.Delay(50).Wait();
            });

            task.Wait();

            //Assert
            Assert.IsTrue(WasCanceled(task1));
            Assert.IsTrue(WasCanceled(task2));
        }

        [Test]
        public void CanStopSpecificTasks()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));

            LogTaskStartEnd(task1);
            LogTaskStartEnd(task2);

            var task = Task.Run(async () =>
            {
                _taskCoordinator.Apply(task1);
                _taskCoordinator.Apply(task2);

                _taskCoordinator.Stop(task1);

                Task.Delay(50).Wait();
            });

            task.Wait();

            //Assert
            Assert.IsTrue(WasCanceled(task1));
            Assert.IsFalse(WasCanceled(task2));
        }

        [Test]
        public void ParallelTasksExecuteAtTheSameTime()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var groupTask = TaskPlexFactory.Parallel(task1, task2);

            LogTaskStartEnd(task1);
            LogTaskStartEnd(task2);
            LogTaskStartEnd(groupTask);

            var task = Task.Run(async () => { await _taskCoordinator.Apply(groupTask).ConfigureAwait(false); });

            task.Wait();

            Assert.IsTrue(StartsBefore(groupTask, task1));
            Assert.IsTrue(EndsBefore(task1, groupTask));
            Assert.IsTrue(EndsBefore(task2, groupTask));
            Assert.Less(Math.Abs((_taskLog[task1].Item1 - _taskLog[task2].Item1).TotalMilliseconds), 10);
        }

        [Test]
        public void SequentialTasksExecuteOneAfterAnother()
        {
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var groupTask = TaskPlexFactory.Sequential(task1, task2);

            LogTaskStartEnd(task1);
            LogTaskStartEnd(task2);
            LogTaskStartEnd(groupTask);

            var task = Task.Run(async () => { await _taskCoordinator.Apply(groupTask).ConfigureAwait(false); });

            task.Wait();

            Assert.IsTrue(StartsBefore(groupTask, task1));
            Assert.IsTrue(EndsBefore(task1, task2));
            Assert.IsTrue(EndsBefore(task2, groupTask));
            Assert.Less(Duration(task1) + Duration(task2), Duration(groupTask));
        }
    }
}