// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsHelper.cs" company="">
// </copyright>
// <summary>
//   Defines the GraphicsHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Utility
{
    using System.Collections.Generic;

    using Finline.Code.Constants;
    using Game.Entities;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The graphics helper.
    /// </summary>
    public static class GraphicsHelper
    {
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
        public static bool IsColliding(this Entity entity, List<EnvironmentObject> environmentObjects, out float distance)
        {
            var colliding = false;
            distance = 1;
            foreach (var obj in environmentObjects)
            {
                float intersection;
                if (!entity.GetBound.Intersection(obj.GetBound, out intersection))
                {
                    continue;
                }

                if (intersection < distance)
                {
                    distance = intersection;
                }

                switch (obj.Type)
                {
                    case GameConstants.EnvObjects.cube:

                        // case GameConstants.EnvObjects.chair:
                        // case GameConstants.EnvObjects.desk:
                        colliding = true;
                        break;
                }
            }

            return colliding;
        }

        /// <summary>
        /// The intersection equation.
        /// </summary>
        /// <param name="sphere1">
        /// The first sphere.
        /// </param>
        /// <param name="sphere2">
        /// The second sphere.
        /// </param>
        /// <param name="distance">
        /// The distance between <paramref name="sphere1"/> and <paramref name="sphere2"/>.
        /// </param>
        /// <returns>
        /// true or false for colliding.
        /// </returns>
        public static bool Intersection(this BoundingSphere sphere1, BoundingSphere sphere2, out float distance)
        {
            float result1;
            Vector3.DistanceSquared(ref sphere1.Center, ref sphere2.Center, out result1);
            var result = (sphere1.Radius + sphere2.Radius)*
                         (sphere1.Radius + sphere2.Radius);
            distance = result1 - result;
            return result1 < result;
        }
    }
}
