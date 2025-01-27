﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Collision.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   Defines the Collision type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Finline.Code.Game.Entities;
    using Finline.Code.Game.Entities.LivingEntity;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The graphics helper.
    /// </summary>
    public static class Collision
    {
        /// <summary>
        /// The tolerance.
        /// </summary>
        private const double Tolerance = 1e-10;

        /// <summary>
        /// The removal flag.
        /// </summary>
        private enum RemovalFlag
        {
            /// <summary>
            /// The none.
            /// </summary>
            None, 

            /// <summary>
            /// The mid point.
            /// </summary>
            MidPoint, 

            /// <summary>
            /// The end point.
            /// </summary>
            EndPoint
        }

        /// <summary>
        /// The is colliding.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="environmentObjects">
        /// The environment objects.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="CollisionResult"/>.
        /// </returns>
        public static CollisionResult IsColliding(this Entity entity, IEnumerable<EnvironmentObject> environmentObjects, Vector2 direction)
        {
            return entity.IsColliding(environmentObjects.Select(obj => (Entity)obj).ToList(), direction);
        }

        /// <summary>
        /// The is colliding.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="bosses">
        /// The bosses.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="CollisionResult"/>.
        /// </returns>
        public static CollisionResult IsColliding(this Entity entity, IEnumerable<Boss> bosses, Vector2 direction)
        {
            return entity.IsColliding(bosses.Select(obj => (Entity)obj).ToList(), direction);
        }

        /// <summary>
        /// The is colliding.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="enemies">
        /// The enemies.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="CollisionResult"/>.
        /// </returns>
        public static CollisionResult IsColliding(this Entity entity, IEnumerable<Enemy> enemies, Vector2 direction)
        {
            return entity.IsColliding(enemies.Select(obj => (Entity)obj).ToList(), direction);
        }

        /// <summary>
        /// The is colliding.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsColliding(this Entity entity, Player player, Vector2 direction)
        {
            return entity.GetBound.PolygonCollision(player.GetBound, direction).WillIntersect;
        }

        /// <summary>
        /// The can see.
        /// </summary>
        /// <param name="ownPosition">
        /// The own position.
        /// </param>
        /// <param name="wantToSee">
        /// The want to see.
        /// </param>
        /// <param name="objectsThatHide">
        /// The objects that hide.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CanSee(this Vector3 ownPosition, Vector3 wantToSee, IEnumerable<Entity> objectsThatHide, float range = 800)
        {
            var direction = wantToSee - ownPosition;
            if (direction.LengthSquared() > range)
            {
                return false;
            }
            
            var bound = new[]
                            {
                                new VertexPositionColor(ownPosition, Color.White), 
                                new VertexPositionColor(ownPosition + direction, Color.White)
                            };
            direction.Normalize();
            var cannotSee = objectsThatHide.Any(obj => (ownPosition - obj.Position).LengthSquared() < range && 
                                                        bound.PolygonCollision(obj.GetBound, direction.Get2D() * 0.01f).WillIntersect);

            return !cannotSee;
        }

        /// <summary>
        /// The get vertices.
        /// </summary>
        /// <param name="modModel">
        /// The mod model.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public static List<Vector3> GetVertices(this Model modModel)
        {
            var verticies = new List<Vector3>();
            var transforms = new Matrix[modModel.Bones.Count];
            modModel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (var mesh in modModel.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    var vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    var vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    var vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData(vertexData);

                    for (var i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        var transformedPosition =
                            Vector3.Transform(
                                new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), 
                                transforms[mesh.ParentBone.Index]);

                        verticies.Add(transformedPosition);
                    }
                }
            }

            return verticies;
        }

        /// <summary>
        /// The get hull.
        /// </summary>
        /// <param name="initialPoints">
        /// The initial points.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// missing removal flag.
        /// </exception>
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

        /// <summary>
        /// Detecting collisions with <paramref name="environmentObjects"/>.
        /// </summary>
        /// <param name="entity">
        /// The entity which is checked for intersections.
        /// </param>
        /// <param name="environmentObjects">
        /// The environment objects that can collide with the <paramref name="entity"/>.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// true or false for colliding.
        /// </returns>
        private static CollisionResult IsColliding(this Entity entity, IEnumerable<Entity> environmentObjects, Vector2 direction)
        {
            var colliding = new CollisionResult();
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

                colliding.HitEntities.Add(obj);
                colliding.Translation = colliding.Translation != null ? colliding.Translation + collision.MinimumTranslationVector : collision.MinimumTranslationVector;
            }

            return colliding;
        }

        /// <summary>
        /// The swap.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <param name="j">
        /// The j.
        /// </param>
        /// <typeparam name="T">
        /// The T.
        /// </typeparam>
        private static void Swap<T>(IList<T> list, int i, int j)
        {
            if (i == j)
            {
                return;
            }

            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// The cross.
        /// </summary>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        /// <param name="p3">
        /// The p 3.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        private static double Ccw(Vector3 p1, Vector3 p2, Vector3 p3)
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

        /// <summary>
        /// The which to remove from boundary.
        /// </summary>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        /// <param name="p3">
        /// The p 3.
        /// </param>
        /// <returns>
        /// The <see cref="RemovalFlag"/>.
        /// </returns>
        private static RemovalFlag WhichToRemoveFromBoundary(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var cross = Ccw(p1, p2, p3);
            if (cross < 0)
            {
                // Remove p2
                return RemovalFlag.MidPoint;
            }

            if (cross > 0)
            {
                // Remove none.
                return RemovalFlag.None;
            }

            // Check for being reversed using the dot product off the difference vectors.
            var dotp = ((p3.X - p2.X) * (p2.X - p1.X)) + ((p3.Y - p2.Y) * (p2.Y - p1.Y));
            if (Math.Abs(dotp) < 1e-10)
            {
                // Remove p2
                return RemovalFlag.MidPoint;
            }

            return dotp < 0 ? RemovalFlag.EndPoint : RemovalFlag.MidPoint;
        }

        /// <summary>
        /// The build edges.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/>.
        /// </returns>
        private static IList<Vector3> BuildEdges(this IReadOnlyList<VertexPositionColor> points)
        {
            var edges = new Vector3[points.Count];
            for (var i = 0; i < points.Count; i++)
            {
                var p1 = points[i];
                var p2 = i + 1 >= points.Count ? points[0] : points[i + 1];

                edges[i] = p2.Position - p1.Position;
            }

            return edges;
        }

        /// <summary>
        /// The center.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private static Vector3 Center(this VertexPositionColor[] points)
        {
            float totalX = 0;
            float totalY = 0;
            for (var i = 0; i < points.Length; i++)
            {
                totalX += points[i].Position.X;
                totalY += points[i].Position.Y;
            }

            return new Vector3(totalX / points.Length, totalY / points.Length, 0);
        }

        /// <summary>
        /// The polygon collision.
        /// Check if polygon A is going to collide with polygon B for the given velocity
        /// </summary>
        /// <param name="polygonA">
        /// The polygon a.
        /// </param>
        /// <param name="polygonB">
        /// The polygon b.
        /// </param>
        /// <param name="velocity">
        /// The velocity.
        /// </param>
        /// <returns>
        /// The <see cref="PolygonCollisionResult"/>.
        /// </returns>
        private static PolygonCollisionResult PolygonCollision(this VertexPositionColor[] polygonA, VertexPositionColor[] polygonB, Vector2 velocity)
        {
            var edgesA = polygonA.BuildEdges();
            var edgesB = polygonB.BuildEdges();

            var result = new PolygonCollisionResult { Intersect = true, WillIntersect = true };

            var edgeCountA = edgesA.Count;
            var edgeCountB = edgesB.Count;
            var minIntervalDistance = float.PositiveInfinity;
            var translationAxis = new Vector2();

            // Loop through all the edges of both polygons
            for (var edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                var edge = edgeIndex < edgeCountA ? edgesA[edgeIndex] : edgesB[edgeIndex - edgeCountA];

                // ===== 1. Find if the polygons are currently intersecting =====

                // Find the axis perpendicular to the current edge
                var axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA;
                float minB;
                float maxA;
                float maxB;
                ProjectPolygon(axis, polygonA, out minA, out maxA);
                ProjectPolygon(axis, polygonB, out minB, out maxB);

                // Check if the polygon projections are currentlty intersecting
                if (IntervalDistance(minA, maxA, minB, maxB) > 0)
                {
                    result.Intersect = false;
                }

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
                if (intervalDistance > 0)
                {
                    result.WillIntersect = false;
                }

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.Intersect && !result.WillIntersect)
                {
                    break;
                }

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation Vector2
                intervalDistance = Math.Abs(intervalDistance);
                if (!(intervalDistance < minIntervalDistance))
                {
                    continue;
                }

                minIntervalDistance = intervalDistance;
                translationAxis = axis;

                var centerA = polygonA.Center();
                var centerB = polygonB.Center();
                var d = centerA.Get2D() - centerB.Get2D();
                if (Vector2.Dot(d, translationAxis) < 0)
                {
                    translationAxis = -translationAxis;
                }
            }

            // The minimum translation Vector2 can be used to push the polygons appart.
            // First moves the polygons by their velocity
            // then move polygonA by MinimumTranslationVector.
            if (result.WillIntersect)
            {
                result.MinimumTranslationVector = translationAxis * minIntervalDistance;
            }

            return result;
        }

        /// <summary>
        /// The interval distance.
        /// Calculate the distance between [minA, maxA] and [minB, maxB]
        /// The distance will be negative if the intervals overlap
        /// </summary>
        /// <param name="minA">
        /// The min a.
        /// </param>
        /// <param name="maxA">
        /// The max a.
        /// </param>
        /// <param name="minB">
        /// The min b.
        /// </param>
        /// <param name="maxB">
        /// The max b.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private static float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }

            return minA - maxB;
        }

        /// <summary>
        /// The project polygon.
        /// Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        /// </summary>
        /// <param name="ax">
        /// The ax.
        /// </param>
        /// <param name="polygon">
        /// The polygon.
        /// </param>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        private static void ProjectPolygon(Vector2 ax, IList<VertexPositionColor> polygon, out float min, out float max)
        {
            var axis = new Vector3(ax, 0);

            // To project a point on an axis use the dot product
            var d = Vector3.Dot(axis, polygon[0].Position);
            min = d;
            max = d;
            for (var i = 0; i < polygon.Count; i++)
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

        /// <summary>
        /// The polygon collision result.
        /// </summary>
        private struct PolygonCollisionResult
        {
            /// <summary>
            /// The will intersect.
            /// </summary>
            public bool WillIntersect; // Are the polygons going to intersect forward in time?

            /// <summary>
            /// The intersect.
            /// </summary>
            public bool Intersect; // Are the polygons currently intersecting

            /// <summary>
            /// The minimum translation vector.
            /// </summary>
            public Vector2 MinimumTranslationVector; // The translation to apply to polygon A to push the polygons appart.
        }

        /// <summary>
        /// The collision result.
        /// </summary>
        public class CollisionResult
        {
            /// <summary>
            /// The hit entities.
            /// </summary>
            public readonly List<Entity> HitEntities = new List<Entity>();

            /// <summary>
            /// Initializes a new instance of the <see cref="CollisionResult" /> class.
            /// </summary>
            public CollisionResult()
            {
                this.Translation = null;
            }

            /// <summary>
            /// Gets or sets the translation.
            /// </summary>
            public Vector2? Translation { get; set; }
        }
    }
}
