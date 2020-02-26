using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Aptacode.TaskPlex.Tests
{
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

        /// <summary>
        /// Returns the duration in milliseconds between the task starting and finishing / canceling.
        /// Returns -1 if the task has not yet started or not yet finished 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private double Duration(BaseTask task)
        {
            var (startTime, endTime, cancelTime) = _taskLog[task];

            if (startTime == DateTime.MinValue)
            {
                return -1;
            }

            if (endTime != DateTime.MinValue)
            {
                return (endTime - startTime).TotalMilliseconds;
            }

            if (cancelTime != DateTime.MinValue)
            {
                return (cancelTime - startTime).TotalMilliseconds;
            }

            return -1;
        }
        /// <summary>
        /// Log the time of the given tasks Start, Canceled and Finished events
        /// </summary>
        /// <param name="task"></param>
        private void LogTaskEvents(BaseTask task)
        {
            _taskLog.Add(task, (DateTime.MinValue, DateTime.MinValue, DateTime.MinValue));
            task.OnStarted += LogStartTime;
            task.OnCancelled += LogCancelTime;
            task.OnFinished += LogEndTime;
        }
        private void LogStartTime(object sender, EventArgs e)
        {
            if (sender is BaseTask task)
            {
                _taskLog[task] = (DateTime.Now, _taskLog[task].Item2, _taskLog[task].Item3);
            }
        }

        private void LogCancelTime(object sender, EventArgs e)
        {
            if (sender is BaseTask task)
            {
                _taskLog[task] = (_taskLog[task].Item1, _taskLog[task].Item2, DateTime.Now);
            }
        }

        private void LogEndTime(object sender, EventArgs e)
        {
            if (sender is BaseTask task)
            {
                _taskLog[task] = (_taskLog[task].Item1, DateTime.Now, _taskLog[task].Item2);
            }
        }
        /// <summary>
        /// Returns true if task a started before task b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool StartsBefore(BaseTask a, BaseTask b) => _taskLog[a].Item1 < _taskLog[b].Item1;
        /// <summary>
        /// Returns true if task a ends before task b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool FinishesBefore(BaseTask a, BaseTask b) => _taskLog[a].Item2 < _taskLog[b].Item2;
        /// <summary>
        /// Returns true if task a was canceled
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private bool WasCanceled(BaseTask a) => _taskLog[a].Item3 > _taskLog[a].Item1;
        /// <summary>
        /// Returns true if task a has finished
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private bool HasFinished(BaseTask a) => _taskLog[a].Item2 > _taskLog[a].Item1;

        [Test]
        public void CanApplyTask()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            LogTaskEvents(task1);

            //Apply
            Task.Run(async () => await _taskCoordinator.Apply(task1)).Wait();

            //Assert
            Assert.IsTrue(HasFinished(task1));
        }


        [Test]
        public void CanPauseAllTasks()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));
            LogTaskEvents(task1);
            LogTaskEvents(task2);

            //Apply
            Task.Run(async () =>
            {
                _ = _taskCoordinator.Apply(task1);
                _ = _taskCoordinator.Apply(task2);

                _taskCoordinator.Pause();
                await Task.Delay(30).ConfigureAwait(false);
                _taskCoordinator.Resume();
                await Task.Delay(50).ConfigureAwait(false);
            }).Wait();

            //Assert
            Assert.Greater(Duration(task1), 40, "Task1 should start and end over a longer period then its duration");
            Assert.Greater(Duration(task2), 40, "Task2 should start and end over a longer period then its duration");
        }

        [Test]
        public void CanPauseSpecificTasks()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));
            LogTaskEvents(task1);
            LogTaskEvents(task2);

            //Apply
            Task.Run(async () =>
            {
                _ = _taskCoordinator.Apply(task1);
                _ = _taskCoordinator.Apply(task2);

                _taskCoordinator.Pause(task1);
                await Task.Delay(30).ConfigureAwait(false);
                _taskCoordinator.Resume(task1);
                await Task.Delay(50).ConfigureAwait(false);
            }).Wait();

            //Assert
            Assert.Greater(Duration(task1), 40, "Task1 should have lasted longer 40ms");
            Assert.Less(Duration(task2), 40, "Task2 should have lasted less 40ms");
        }

        [Test]
        public void CanReset()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));
            LogTaskEvents(task1);
            LogTaskEvents(task2);

            //Apply
            Task.Run(async () =>
            {
                _ = _taskCoordinator.Apply(task1);
                _taskCoordinator.Reset();
                _ = _taskCoordinator.Apply(task2);

                await Task.Delay(50).ConfigureAwait(false);
            }).Wait();

            //Assert
            Assert.IsTrue(WasCanceled(task1), "Task1 should have been canceled");
            Assert.IsFalse(WasCanceled(task2), "Task2 should not have been canceled");
        }

        [Test]
        public void CanStopAllTasks()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));
            LogTaskEvents(task1);
            LogTaskEvents(task2);

            //Apply
            Task.Run(async () =>
            {
                _ = _taskCoordinator.Apply(task1);
                _ = _taskCoordinator.Apply(task2);

                _taskCoordinator.Stop();

                await Task.Delay(50).ConfigureAwait(false);
            }).Wait();

            //Assert
            Assert.IsTrue(WasCanceled(task1), "Task1 should have been canceled");
            Assert.IsTrue(WasCanceled(task2), "Task2 should have been canceled");
        }

        [Test]
        public void CanStopSpecificTasks()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(20));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(20));

            LogTaskEvents(task1);
            LogTaskEvents(task2);

            //Apply
            Task.Run(async () =>
            {
                _ = _taskCoordinator.Apply(task1);
                _ = _taskCoordinator.Apply(task2);

                _taskCoordinator.Stop(task1);

                await Task.Delay(50).ConfigureAwait(false);
            }).Wait();

            //Assert
            Assert.IsTrue(WasCanceled(task1), "Task1 should have been canceled");
            Assert.IsFalse(WasCanceled(task2), "Task2 should not have been canceled");
        }

        [Test]
        public void ParallelTasksExecuteAtTheSameTime()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var groupTask = TaskPlexFactory.Parallel(task1, task2);

            LogTaskEvents(task1);
            LogTaskEvents(task2);
            LogTaskEvents(groupTask);

            //Apply
            Task.Run(async () => await _taskCoordinator.Apply(groupTask).ConfigureAwait(false)).Wait();

            //Assert
            Assert.IsTrue(StartsBefore(groupTask, task1));
            Assert.IsTrue(FinishesBefore(task1, groupTask));
            Assert.IsTrue(FinishesBefore(task2, groupTask));
            Assert.Less(Math.Abs((_taskLog[task1].Item1 - _taskLog[task2].Item1).TotalMilliseconds), 10);
        }

        [Test]
        public void SequentialTasksExecuteOneAfterAnother()
        {
            //Arrange
            var task1 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var task2 = new DummyTask(TimeSpan.FromMilliseconds(30));
            var groupTask = TaskPlexFactory.Sequential(task1, task2);

            LogTaskEvents(task1);
            LogTaskEvents(task2);
            LogTaskEvents(groupTask);

            //Apply
            Task.Run(async () => await _taskCoordinator.Apply(groupTask).ConfigureAwait(false)).Wait();

            //Assert
            Assert.IsTrue(StartsBefore(groupTask, task1));
            Assert.IsTrue(FinishesBefore(task1, task2));
            Assert.IsTrue(FinishesBefore(task2, groupTask));
            Assert.Less(Duration(task1) + Duration(task2), Duration(groupTask));
        }
    }
}