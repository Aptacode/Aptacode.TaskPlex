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
                TransformationCoordinator();
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void TransformationCoordinator()
        {
            while (IsRunning)
            {
                lock (mutex)
                {
                    //Start all transformations whose target does not collide with running transformations
                    List<Transformation.Transformation> startedTransformations = new List<Aptacode_PropertyTransposer.Transformation.Transformation>();
                    foreach (var item in pendingTransformations)
                    {
                        if (!runningTransformations.Exists(t => t.Target == item.Target && t.Property == item.Property))
                        {
                            runningTransformations.Add(item);
                            startedTransformations.Add(item);
                            //Remove the transformation from the running list when it finishes.
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

                    //Remove each started transformation from the pending transformation list
                    foreach (var item in startedTransformations)
                    {
                        pendingTransformations.Remove(item);
                    }
                }
                Thread.Sleep(10);
            }
        }
    }
}
