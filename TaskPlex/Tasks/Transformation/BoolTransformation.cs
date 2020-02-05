using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class BoolTransformation : PropertyTransformation<bool>
    {
        /// <summary>
        /// Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="endValue"></param>
        /// <param name="valueUpdater"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public BoolTransformation(object target,
                                    string property,
                                    Func<bool> startValue,
                                    Func<bool> endValue,
                                    Action<bool> valueUpdater,
                                    TimeSpan taskDuration) : base(target,
                                                                  property,
                                                                  startValue,
                                                                  endValue,
                                                                  valueUpdater,
                                                                  taskDuration,
                                                                  RefreshRate.Low)
        { }

        protected override async Task InternalTask()
        {
            await Task.Delay(Duration, CancellationToken.Token).ConfigureAwait(false);
            SetValue(GetEndValue());
        }
    }
}