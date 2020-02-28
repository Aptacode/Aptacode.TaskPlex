using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Interpolators
{
    public abstract class AbstractInterpolator<TType, TVector> : IInterpolator<TType>
    {
        public IEnumerable<TType> Interpolate(int stepCount, EaserFunction easer, params TType[] points)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            //Get the list of edges
            var edges = GetEdges(points);

            //The distance to travel per step
            var velocity = TotalEdgeLength(edges) / stepCount;

            if (velocity <= 0)
            {
                for (var i = 0; i < stepCount; i++)
                {
                    yield return points.Last();
                }
            }
            else
            {
                //Yield a point for each step
                foreach (var (pointA, pointB, edge) in edges)
                {
                    var normal = Normalize(edge);
                    var edgeLength = GetLength(edge);
                    var edgeSteps = GetLength(edge) / velocity;
                    for (var stepIndex = 1; stepIndex < edgeSteps; stepIndex++)
                    {
                        var incrementLength = edgeLength * easer(stepIndex, (int) edgeSteps);
                        var newPoint = Add(pointA, Multiply(normal, incrementLength));

                        yield return FromVector(newPoint);
                    }

                    yield return FromVector(pointB);
                }
            }
        }

        private List<(TVector, TVector, TVector)> GetEdges(IEnumerable<TType> keyPoints)
        {
            var keyPointList = keyPoints.Select(ToVector).ToList();
            //Point A, Point B, Length
            var edges = new List<(TVector, TVector, TVector)>();

            for (var i = 1; i < keyPointList.Count; i++)
            {
                edges.Add((keyPointList[i - 1], keyPointList[i], Subtract(keyPointList[i], keyPointList[i - 1])));
            }

            return edges;
        }

        public float TotalEdgeLength(IEnumerable<(TVector, TVector, TVector)> edges)
        {
            return edges.Sum(edge => GetLength(edge.Item3));
        }

        public abstract float GetLength(TVector vector);
        public abstract TVector Subtract(TVector from, TVector to);
        public abstract TVector Add(TVector from, TVector to);
        public abstract TVector Multiply(TVector from, float value);

        public abstract TVector Normalize(TVector from);

        public abstract TVector ToVector(TType value);
        public abstract TType FromVector(TVector value);
    }
}