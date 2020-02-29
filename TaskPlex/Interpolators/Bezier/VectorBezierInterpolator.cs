using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Interpolators.Easers;

namespace Aptacode.TaskPlex.Interpolators.Bezier
{
    public abstract class VectorBezierInterpolator<TType> : IInterpolator<TType>
    {
        public IEnumerable<TType> Interpolate(int stepCount, EaserFunction easer, params TType[] points)
        {
            if (stepCount <= 0)
            {
                yield break;
            }

            var curveSegments = new List<(Vector2, Vector2, Vector2, Vector2)>();
            var vecPoints = points.Select(ToVector).ToList();

            for (var i = 2; i + 1 < vecPoints.Count; i += 2)
            {
                var point1 = vecPoints[i - 2];
                var point2 = vecPoints[i - 1];
                var point3 = vecPoints[i];
                var point4 = vecPoints[i + 1];

                curveSegments.Add((point1, point2, point3, point4));
            }

            var pointsOnCurve = new List<Vector2>();

            foreach (var curveSegment in curveSegments)
            {
                for (var i = 0; i <= 10; i++)
                {
                    var x = X(i / 10f, curveSegment.Item1.X, curveSegment.Item2.X, curveSegment.Item3.X,
                        curveSegment.Item4.X);
                    var y = Y(i / 10f, curveSegment.Item1.Y, curveSegment.Item2.Y, curveSegment.Item3.Y,
                        curveSegment.Item4.Y);

                    pointsOnCurve.Add(new Vector2(x, y));
                }
            }

            //Get the list of edges
            var edges = GetEdges(pointsOnCurve);

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

        private List<(Vector2, Vector2, Vector2)> GetEdges(List<Vector2> keyPoints)
        {
            //Point A, Point B, Length
            var edges = new List<(Vector2, Vector2, Vector2)>();

            for (var i = 1; i < keyPoints.Count; i++)
            {
                edges.Add((keyPoints[i - 1], keyPoints[i], Subtract(keyPoints[i], keyPoints[i - 1])));
            }

            return edges;
        }

        public float TotalEdgeLength(IEnumerable<(Vector2, Vector2, Vector2)> edges)
        {
            return edges.Sum(edge => GetLength(edge.Item3));
        }

        public Vector2 Subtract(Vector2 from, Vector2 to)
        {
            return from - to;
        }

        public Vector2 Normalize(Vector2 from)
        {
            return Vector2.Normalize(from);
        }

        public float GetLength(Vector2 vector)
        {
            return vector.Length();
        }

        public Vector2 Add(Vector2 from, Vector2 to)
        {
            return Vector2.Add(from, to);
        }

        public Vector2 Multiply(Vector2 from, float value)
        {
            return Vector2.Multiply(from, value);
        }

        // Parametric functions for drawing a degree 3 Bezier curve.
        private static float X(float t,
            float x0, float x1, float x2, float x3)
        {
            return (float) (
                x0 * Math.Pow(1 - t, 3) +
                x1 * 3 * t * Math.Pow(1 - t, 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }

        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float) (
                y0 * Math.Pow(1 - t, 3) +
                y1 * 3 * t * Math.Pow(1 - t, 2) +
                y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                y3 * Math.Pow(t, 3)
            );
        }

        public abstract Vector2 ToVector(TType value);
        public abstract TType FromVector(Vector2 value);
    }
}