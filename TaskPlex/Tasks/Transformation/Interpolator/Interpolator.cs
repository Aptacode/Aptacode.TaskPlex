using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.Easing;
using Aptacode.TaskPlex.Tasks.Transformation.Interpolator.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Aptacode.TaskPlex.Tasks.Transformation.Interpolator
{
    public abstract class Interpolator<T> : BaseTask
    {
        readonly Easer _easer;
        readonly Stopwatch _stepTimer;

        protected Interpolator(T startValue, T endValue, TimeSpan duration, RefreshRate refreshRate) : this(startValue,
                                                                                                              endValue,
                                                                                                              duration,
                                                                                                              refreshRate,
                                                                                                              new LinearEaser())
        { }

        protected Interpolator(T startValue, T endValue, TimeSpan duration, RefreshRate refreshRate, Easer easer) : base(duration)
        {
            _stepTimer = new Stopwatch();
            _easer = easer;

            CurrentValue = StartValue = startValue;
            EndValue = endValue;
            RefreshRate = refreshRate;
        }

        /// <summary>
        /// Returns the Easing function
        /// </summary>

        public RefreshRate RefreshRate { get; set; }

        T StartValue { get; }

        T CurrentValue { get; set; }

        T EndValue { get; }

        public List<T> GetValues()
        {
            var values = new List<T>();
            var stepCount = GetStepCount();
            var incrementValue = GetIncrementValue(StartValue, EndValue, stepCount);
            var incrementIndex = 0;
            var currentValue = StartValue;

            for(var stepIndex = 1; stepIndex < stepCount; stepIndex++)
            {
                var nextIncrementIndex = GetNextIncrementIndex(stepIndex, stepCount);
                currentValue = NextValue(currentValue, incrementIndex, incrementValue, nextIncrementIndex);
                values.Add(currentValue);
                incrementIndex = nextIncrementIndex;
            }

            return values;
        }

        public event EventHandler<InterpolationValueChangedEventArgs<T>> OnValueChanged;

        protected abstract T Subtract(T a, T b);

        protected abstract T Add(T a, T b);

        protected abstract T Divide(T a, int incrementCount);

        protected override async Task InternalTask() => await new TaskFactory(CancellationToken.Token).StartNew(() =>
                                                        {
                                                            _stepTimer.Restart();
                                                            var stepCount = GetStepCount();
                                                            var incrementValue = GetIncrementValue(StartValue,
                                                                                                   EndValue,
                                                                                                   stepCount);
                                                            var incrementIndex = 0;

                                                            for(var stepIndex = 1; stepIndex < stepCount; stepIndex++)
                                                            {
                                                                var nextIncrementIndex = GetNextIncrementIndex(stepIndex,
                                                                                                               stepCount);

                                                                CurrentValue = NextValue(CurrentValue,
                                                                                         incrementIndex,
                                                                                         incrementValue,
                                                                                         nextIncrementIndex);
                                                                OnValueChanged?.Invoke(this,
                                                                                       new InterpolationValueChangedEventArgs<T>(CurrentValue));
                                                                incrementIndex = nextIncrementIndex;

                                                                DelayAsync(stepIndex).Wait();
                                                            }

                                                            OnValueChanged?.Invoke(this,
                                                                                   new InterpolationValueChangedEventArgs<T>(EndValue));
                                                        })
            .ConfigureAwait(false);

        int GetStepCount()
        {
            var taskDurationMilliSeconds = (int)Duration.TotalMilliseconds;
            var stepDurationMilliSeconds = (int)RefreshRate;
            return (int)Math.Floor(((double)taskDurationMilliSeconds) / stepDurationMilliSeconds);
        }

        T GetIncrementValue(T startValue, T endValue, int stepCount)
        {
            var difference = Subtract(endValue, startValue);

            return (stepCount <= 1) ? difference : Divide(difference, stepCount);
        }

        int GetNextIncrementIndex(int stepIndex, int stepCount) => (int)Math.Floor(_easer.ProgressAt(stepIndex,
                                                                                                     stepCount) *
            stepCount);

        T NextValue(T currentValue, int incrementIndex, T incrementValue, int nextIncrementIndex)
        {
            while(incrementIndex < nextIncrementIndex)
            {
                incrementIndex++;
                currentValue = Add(currentValue, incrementValue);
            }

            return currentValue;
        }

        async Task DelayAsync(int currentStep)
        {
            var millisecondsAhead =
                (int)(((int)RefreshRate * currentStep) - _stepTimer.ElapsedMilliseconds);
            //the Task.Delay function will only accuratly sleep for >8ms
            if(millisecondsAhead > 8)
            {
                await Task.Delay(millisecondsAhead).ConfigureAwait(false);
            }
        }
    }
}