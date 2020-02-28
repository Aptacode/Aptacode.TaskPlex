using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Interpolators
{
    public class IntInterpolator : IInterpolator<int>
    {
        public IEnumerable<int> Interpolate(int stepCount, EaserFunction easer, params int[] points)
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
                    var edgeSteps = Math.Abs(edge) / velocity;
                    for (var stepIndex = 1; stepIndex < edgeSteps; stepIndex++)
                    {
                        yield return pointA + (int) (edge * easer(stepIndex, edgeSteps));
                    }

                    yield return pointB;
                }
            }
        }

        private List<(int, int, int)> GetEdges(IEnumerable<int> keyPoints)
        {
            var keyPointList = keyPoints.ToList();
            //Point A, Point B, Length
            var edges = new List<(int, int, int)>();

            for (var i = 1; i < keyPointList.Count; i++)
            {
                edges.Add((keyPointList[i - 1], keyPointList[i], keyPointList[i] - keyPointList[i - 1]));
            }

            return edges;
        }

        public int TotalEdgeLength(IEnumerable<(int, int, int)> edges)
        {
            return edges.Sum(edge => Math.Abs(edge.Item3));
        }
    }
}