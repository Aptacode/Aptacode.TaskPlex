using System;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation
{
    public class StringTransformation : PropertyTransformation<string>
    {
        /// <summary>
        ///     Update a string property on the target to the value returned by the given Func after the task duration
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="endValue"></param>
        /// <param name="valueUpdater"></param>
        /// <param name="taskDuration"></param>
        /// <param name="stepDuration"></param>
        public StringTransformation(
            object target,
            string property,
            Func<string> startValue,
            Func<string> endValue,
            Action<string> valueUpdater,
            TimeSpan taskDuration,
            TimeSpan stepDuration) : base(target, property, startValue,endValue, valueUpdater, taskDuration, stepDuration)
        {
        }

        protected override async Task InternalTask()
        {
            await Task.Delay(Duration, CancellationToken.Token).ConfigureAwait(false);
            SetValue(GetEndValue());
        }
    }
}