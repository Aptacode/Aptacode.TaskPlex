using System;
using System.Collections.Generic;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easers;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public abstract class Interpolator<T>
    {

        public List<T> Interpolate(T startValue, T endValue, int stepCount, Easer easer)
        {
            var values = new List<T>();

            if (stepCount == 0)
                return values;

            var incrementValue = GetIncrementValue(startValue, endValue, stepCount);
            var incrementIndex = 0;
            var currentValue = startValue;

            for (var stepIndex = 1; stepIndex < stepCount; stepIndex++)
            {
                var nextIncrementIndex = GetNextIncrementIndex(stepIndex, stepCount, easer);
                currentValue = NextValue(currentValue, incrementIndex, incrementValue, nextIncrementIndex);
                values.Add(currentValue);
                incrementIndex = nextIncrementIndex;
            }

            values.Add(endValue);

            return values;
        }

        protected abstract T Subtract(T a, T b);

        protected abstract T Add(T a, T b);

        protected abstract T Divide(T a, int incrementCount);

        private T GetIncrementValue(T startValue, T endValue, int stepCount)
        {
            var difference = Subtract(endValue, startValue);

            return (stepCount <= 1) ? difference : Divide(difference, stepCount);
        }

        private static int GetNextIncrementIndex(int stepIndex, int stepCount, Easer easer) => (int)Math.Floor(easer.ProgressAt(stepIndex, stepCount) * stepCount);

        private T NextValue(T currentValue, int incrementIndex, T incrementValue, int nextIncrementIndex)
        {
            while(incrementIndex < nextIncrementIndex)
            {
                incrementIndex++;
                currentValue = Add(currentValue, incrementValue);
            }

            return currentValue;
        }
    }
}