using Aptacode.Core.Tasks.Transformations.Interpolation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Aptacode.Core.Tasks.Transformations
{
    public class ColorTransformationventArgs : BaseTaskEventArgs
    {
    }

    public class ColorTransformation : PropertyTransformation<Color>
    {
        public ColorTransformation(object target, string property, Func<Color> destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {
        }

        public ColorTransformation(object target, string property, Color destinationValue, TimeSpan taskDuration, TimeSpan stepDuration) : base(target, property, destinationValue, taskDuration, stepDuration)
        {

        }

        private static readonly Object mutex = new Object();
        Queue<int> a, r, g, b;

        public override async Task StartAsync()
        {
            RaiseOnStarted(new ColorTransformationventArgs());

            Color startValue = GetStartValue();
            Color endValue = GetEndValue();
            a = new Queue<int>();
            r = new Queue<int>();
            g = new Queue<int>();
            b = new Queue<int>();

            IntInterpolator aComponentInterpolator = new IntInterpolator(startValue.A, endValue.A, Duration, StepDuration);
            IntInterpolator rComponentInterpolator = new IntInterpolator(startValue.R, endValue.R, Duration, StepDuration);
            IntInterpolator gComponentInterpolator = new IntInterpolator(startValue.G, endValue.G, Duration, StepDuration);
            IntInterpolator bComponentInterpolator = new IntInterpolator(startValue.B, endValue.B, Duration, StepDuration);

            aComponentInterpolator.OnValueChanged += (s, e) =>
            {
                lock (mutex)
                {
                    a.Enqueue(e.Value);
                    componentUpdated();
                }
            };

            rComponentInterpolator.OnValueChanged += (s, e) =>
            {
                lock (mutex)
                {
                    r.Enqueue(e.Value);
                    componentUpdated();
                }
            };

            gComponentInterpolator.OnValueChanged += (s, e) =>
            {
                lock (mutex)
                {
                    g.Enqueue(e.Value);
                    componentUpdated();
                }
            };

            bComponentInterpolator.OnValueChanged += (s, e) =>
            {
                lock (mutex)
                {
                    b.Enqueue(e.Value);
                    componentUpdated();
                }
            };

            await Task.WhenAll(aComponentInterpolator.StartAsync(), rComponentInterpolator.StartAsync(), gComponentInterpolator.StartAsync(), bComponentInterpolator.StartAsync());

            RaiseOnFinished(new ColorTransformationventArgs());
        }

        private void componentUpdated()
        {
            if (a.Count > 0 && r.Count > 0 && g.Count > 0 && b.Count >0)
            {
                SetValue(Color.FromArgb(a.Dequeue(), r.Dequeue(), g.Dequeue(), b.Dequeue()));
            }
        }
    }
}
