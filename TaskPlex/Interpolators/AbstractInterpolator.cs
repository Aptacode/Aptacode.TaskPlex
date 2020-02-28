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

            //Calculate the total length to travel
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
                foreach (var (pointA, pointB, edge) in edges)
                {
                    //Calculate the unit vector for the edge
                    var normal = Normalize(edge);
                    //Determine the length of the edge
                    var edgeLength = GetLength(edge);
                    //Calculate the number of steps needed for this edge
                    var edgeSteps = edgeLength / velocity;
                    //Yield a value at each step along this edge
                    for (var stepIndex = 1; stepIndex < edgeSteps; stepIndex++)
                    {
                        //Calculate the progress through this edge
                        var progress = easer(stepIndex, (int) edgeSteps);
                        //Multiply the edge's distance by the progress
                        var progressDistance = edgeLength * progress;
                        //Multiply the normal by the progress to get the vector from the last point
                        var progressVector = Multiply(normal, progressDistance);
                        //Add the progress vector to the last point to get the new point
                        var newPoint = Add(pointA, progressVector);
                        //Convert and return
                        yield return FromVector(newPoint);
                    }

                    //Return the end point for this edge
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