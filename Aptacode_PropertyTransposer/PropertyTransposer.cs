using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode_PropertyTransposer
{
    public class PropertyTransposer
    {
        public List<Transformation.Transformation> pendingTransformations;
        public List<Transformation.Transformation> runningTransformations;
        private static readonly Object mutex = new Object();
        public bool IsRunning { get; set; }

        public PropertyTransposer()
        {
            pendingTransformations = new List<Transformation.Transformation>();
            runningTransformations = new List<Transformation.Transformation>();
            IsRunning = false;
        }

        public void Apply(Transformation.Transformation transformation)
        {
            pendingTransformations.Add(transformation);
        }

        public void Start()
        {
            IsRunning = true;

            new TaskFactory().StartNew(() =>
            {
                while (IsRunning)
                {
                    lock (mutex)
                    {
                        List<Transformation.Transformation> startedTransformations = new List<Aptacode_PropertyTransposer.Transformation.Transformation>();
                        foreach (var item in pendingTransformations)
                        {
                            if (!runningTransformations.Exists(t => t.Target == item.Target && t.Property == item.Property))
                            {
                                runningTransformations.Add(item);
                                startedTransformations.Add(item);
                                item.OnFinished += (s, e) =>
                                {
                                    lock (mutex)
                                    {
                                        runningTransformations.Remove((Transformation.Transformation)s);
                                    }
                                };
                                item.Start();
                            }
                        }

                        foreach (var item in startedTransformations)
                        {
                            pendingTransformations.Remove(item);
                        }
                    }

                    Thread.Sleep(10);
                }
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
