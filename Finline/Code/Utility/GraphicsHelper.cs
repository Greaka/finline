using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Finline.Code.Constants;
using Finline.Code.Game.Entities;
using Microsoft.Xna.Framework;

namespace Finline.Code.Utility
{
    public static class GraphicsHelper
    {
        public static bool isColliding(this Entity entity, ConcurrentDictionary<int, EnvironmentObject> environmentObjects, out float distance)
        {
            var colliding = false;
            distance = -1;
            for (var i=0; i<environmentObjects.Values.Count; i++)
            {
                var obj = environmentObjects[i];
                float intersection;
                if (!entity.GetBound.Intersection(obj.GetBound, out intersection)) continue;
                if (intersection > distance) distance = intersection;
                switch (obj.Type)
                {
                    case GameConstants.EnvObjects.bottle_cap2:
                        break;
                    case GameConstants.EnvObjects.cube:
                        colliding = true;
                        break;
                }
            }

            return colliding;
        }

        public static bool Intersection(this BoundingSphere sphere1, BoundingSphere sphere2, out float distance)
        {
            float result1;
            Vector3.DistanceSquared(ref sphere1.Center, ref sphere2.Center, out result1);
            var result = (sphere1.Radius + sphere2.Radius)*
                         (sphere1.Radius + sphere2.Radius);
            distance = result1 - result;
            return result1 > result;
        }
    }
}
