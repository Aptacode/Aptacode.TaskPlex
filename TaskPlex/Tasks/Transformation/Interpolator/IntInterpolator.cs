using System;
using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public class IntInterpolator : Interpolator<int>
    {
        public IEnumerable<int> Interpolate(int startValue, int endValue, int stepCount, Easer easer)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            var incrementValue = GetIncrementValue(startValue, endValue, stepCount);
            var incrementIndex = 0;
            var currentValue = startValue;

            var totalDifference = endValue - startValue;
            var calculatedEndValue = incrementValue * stepCount;
            var incrementDifference = totalDifference - calculatedEndValue;

            var repeatStepIndex = stepCount + 1;
            if (incrementValue != 0)
            {
                var repeatedStepCount = incrementDifference / incrementValue;
                if (repeatedStepCount > 0)
                {
                    repeatStepIndex = (int) Math.Ceiling((double) stepCount / repeatedStepCount);
                }
            }


            for (var stepIndex = 1; stepIndex < stepCount; stepIndex++)
            {
                var nextIncrementIndex = GetNextIncrementIndex(stepIndex, stepCount, easer);
                currentValue = NextValue(currentValue, incrementIndex,
                    incrementValue * (stepIndex % repeatStepIndex == 0 ? 2 : 1), nextIncrementIndex);
                incrementIndex = nextIncrementIndex;

                yield return currentValue;
            }

            yield return endValue;
        }

        private int GetIncrementValue(int startValue, int endValue, int stepCount)
        {
            var difference = endValue - startValue;
            return stepCount <= 1 ? difference : difference / stepCount;
        }

        private static int GetNextIncrementIndex(int stepIndex, int stepCount, Easer easer)
        {
            return (int) Math.Floor(easer.ProgressAt(stepIndex, stepCount) * stepCount);
        }

        private int NextValue(int currentValue, int incrementIndex, int incrementValue, int nextIncrementIndex)
        {
            while (incrementIndex < nextIncrementIndex)
            {
                incrementIndex++;
                currentValue += incrementValue;
            }

            return currentValue;
        }
    }
}