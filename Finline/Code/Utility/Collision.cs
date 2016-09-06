// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Collision.cs" company="">
// </copyright>
// <summary>
//   Defines the Collision type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Finline.Code.Constants;
    using Game.Entities;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The graphics helper.
    /// </summary>
    public static class Collision
    {
        private const double Tolerance = 1e-10;

        /// <summary>
        /// Detecting collisions with <paramref name="environmentObjects"/>.
        /// </summary>
        /// <param name="entity">
        /// The entity which is checked for intersections.
        /// </param>
        /// <param name="environmentObjects">
        /// The environment objects that can collide with the <paramref name="entity"/>.
        /// </param>
        /// <param name="distance">
        /// The distance until the closest object. Can be lesser than zero.
        /// </param>
        /// <returns>
        /// true or false for colliding.
        /// </returns>
        public static Vector2? IsColliding(this Entity entity, List<EnvironmentObject> environmentObjects, Vector2 direction)
        {
            Vector2? colliding = null;
            foreach (var obj in environmentObjects)
            {
                if (!((entity.Position - obj.Position).LengthSquared() < 64))
                {
                    continue;
                }

                var collision = entity.GetBound.PolygonCollision(obj.GetBound, direction);
                if (!collision.WillIntersect)
                {
                    continue;
                }
            }

            return colliding;
        }

        public static List<Vector3> GetVerticies(this Model modModel)
        {
            var verticies = new List<Vector3>();
            foreach (var modmModel in modModel.Meshes)
            {
                foreach (var mmpModel in modmModel.MeshParts)
                {
                    var arrVectors = new Vector3[mmpModel.NumVertices * 2];
                    mmpModel.VertexBuffer.GetData<Vector3>(arrVectors);
                    verticies.AddRange(arrVectors);
                }
            }

            return verticies;
        }

        public static IList<Vector3> GetHull(this IList<Vector3> initialPoints)
        {
            if (initialPoints.Count < 2)
            {
                return initialPoints.ToList();
            }

            // Find point with minimum y; if more than one, minimize x also.
            var minI = Enumerable.Range(0, initialPoints.Count).Aggregate(
                (jMin, jCur) =>
                    {
                        if (initialPoints[jCur].Y < initialPoints[jMin].Y)
                        {
                            return jCur;
                        }

                        if (initialPoints[jCur].Y > initialPoints[jMin].Y)
                        {
                            return jMin;
                        }

                        return initialPoints[jCur].X < initialPoints[jMin].X ? jCur : jMin;
                    });

            // Sort them by polar angle from minI, 
            var sortQuery = Enumerable.Range(0, initialPoints.Count).Where(i => i != minI) // Skip the min point
                .Select(
                    i =>
                    new KeyValuePair<double, Vector3>(
                        Math.Atan2(
                            initialPoints[i].Y - initialPoints[minI].Y, 
                            initialPoints[i].X - initialPoints[minI].X), 
                        initialPoints[i])).OrderBy(pair => pair.Key).Select(pair => pair.Value);
            var points = new List<Vector3>(initialPoints.Count) { initialPoints[minI] };

            // Add minimum point
            points.AddRange(sortQuery); // Add the sorted points.

            var m = 0;
            for (int i = 1, n = points.Count; i < n; i++)
            {
                var keepNewPoint = true;
                if (m == 0)
                {
                    // Find at least one point not coincident with points[0]
                    keepNewPoint = !(points[0] == points[i]);
                }
                else
                {
                    while (true)
                    {
                        var flag = WhichToRemoveFromBoundary(points[m - 1], points[m], points[i]);
                        if (flag == RemovalFlag.None)
                        {
                            break;
                        }

                        if (flag == RemovalFlag.MidPoint)
                        {
                            if (m > 0)
                            {
                                m--;
                            }

                            if (m == 0)
                            {
                                break;
                            }
                        }
                        else if (flag == RemovalFlag.EndPoint)
                        {
                            keepNewPoint = false;
                            break;
                        }
                        else
                        {
                            throw new Exception("Unknown RemovalFlag");
                        }
                    }
                }

                if (!keepNewPoint)
                {
                    continue;
                }

                m++;
                Swap(points, m, i);
            }

            // points[M] is now the last point in the boundary.  Remove the remainder.
            points.RemoveRange(m + 1, points.Count - m - 1);
            return points;
        }

        static void Swap<T>(IList<T> list, int i, int j)
        {
            if (i == j)
            {
                return;
            }

            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        static double Ccw(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // Compute (p2 - p1) X (p3 - p1)
            double cross1 = (p2.X - p1.X) * (p3.Y - p1.Y);
            double cross2 = (p2.Y - p1.Y) * (p3.X - p1.X);
            if (Math.Abs(cross1 - cross2) < Tolerance)
            {
                return 0;
            }

            return cross1 - cross2;
        }

        enum RemovalFlag
        {
            None, 
            MidPoint, 
            EndPoint
        }

        static RemovalFlag WhichToRemoveFromBoundary(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var cross = Ccw(p1, p2, p3);
            if (cross < 0)

                // Remove p2
                return RemovalFlag.MidPoint;
            if (cross > 0)

                // Remove none.
                return RemovalFlag.None;

            // Check for being reversed using the dot product off the difference vectors.
            var dotp = (p3.X - p2.X) * (p2.X - p1.X) + (p3.Y - p2.Y) * (p2.Y - p1.Y);
            if (dotp == 0.0)

                // Remove p2
                return RemovalFlag.MidPoint;
            if (dotp < 0)

                // Remove p3
                return RemovalFlag.EndPoint;
            else

            // Remove p2
                return RemovalFlag.MidPoint;
        }

        public struct PolygonCollisionResult
        {
            public bool WillIntersect; // Are the polygons going to intersect forward in time?
            public bool Intersect; // Are the polygons currently intersecting
            public Vector2 MinimumTranslationVector; // The translation to apply to polygon A to push the polygons appart.
        }

        private static IList<Vector3> BuildEdges(this VertexPositionColor[] points)
        {
            var edges = new Vector3[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                var p1 = points[i];
                var p2 = i + 1 >= points.Length ? points[0] : points[i + 1];

                edges[i] = p2.Position - p1.Position;
            }

            return edges;
        }

        public static Vector3 Center(this VertexPositionColor[] points)
        {
            float totalX = 0;
            float totalY = 0;
            for (var i = 0; i < points.Length; i++)
            {
                totalX += points[i].Position.X;
                totalY += points[i].Position.Y;
            }

            return new Vector3(totalX / (float)points.Length, totalY / (float)points.Length, 0);
        }

        // Check if polygon A is going to collide with polygon B for the given velocity
        public static PolygonCollisionResult PolygonCollision(this VertexPositionColor[] polygonA, VertexPositionColor[] polygonB, Vector2 velocity)
        {
            var edgesA = polygonA.BuildEdges();
            var edgesB = polygonB.BuildEdges();

            var result = new PolygonCollisionResult();
            result.Intersect = true;
            result.WillIntersect = true;

            var edgeCountA = edgesA.Count;
            var edgeCountB = edgesB.Count;
            var minIntervalDistance = float.PositiveInfinity;
            var translationAxis = new Vector2();
            Vector3 edge;

            // Loop through all the edges of both polygons
            for (var edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = edgesA[edgeIndex];
                }
                else
                {
                    edge = edgesB[edgeIndex - edgeCountA];
                }

                // ===== 1. Find if the polygons are currently intersecting =====

                // Find the axis perpendicular to the current edge
                var axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting
                if (IntervalDistance(minA, maxA, minB, maxB) > 0) result.Intersect = false;

                // ===== 2. Now find if the polygons *will* intersect =====

                // Project the velocity on the current axis
                var velocityProjection = Vector2.Dot(axis, velocity);

                // Get the projection of polygon A during the movement
                if (velocityProjection < 0)
                {
                    minA += velocityProjection;
                }
                else
                {
                    maxA += velocityProjection;
                }

                // Do the same test as above for the new projection
                var intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance > 0) result.WillIntersect = false;

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.Intersect && !result.WillIntersect) break;

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation Vector2
                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {
                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;

                    var centerA = polygonA.Center();
                    var centerB = polygonB.Center();
                    Vector2 d = centerA.Get2D() - centerB.Get2D();
                    if (Vector2.Dot(d, translationAxis) < 0) translationAxis = -translationAxis;
                }
            }

            // The minimum translation Vector2 can be used to push the polygons appart.
            // First moves the polygons by their velocity
            // then move polygonA by MinimumTranslationVector.
            if (result.WillIntersect) result.MinimumTranslationVector = translationAxis * minIntervalDistance;

            return result;
        }

        // Calculate the distance between [minA, maxA] and [minB, maxB]
        // The distance will be negative if the intervals overlap
        public static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        // Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        public static void ProjectPolygon(Vector2 ax, VertexPositionColor[] polygon, ref float min, ref float max)
        {
            var axis = new Vector3(ax, 0);

            // To project a point on an axis use the dot product
            var d = Vector3.Dot(axis, polygon[0].Position);
            min = d;
            max = d;
            for (var i = 0; i < polygon.Length; i++)
            {
                d = Vector3.Dot(polygon[i].Position, axis);
                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }
    }
}
