using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using prototyp.Code.Constants;
using prototyp.Code.Game.Entities;

namespace prototyp.Code.Utility
{
    public static class GraphicsHelper
    {
        public static bool isColliding(this Entity entity, ConcurrentDictionary<int, EnvironmentObject> environmentObjects)
        {
            var colliding = false;
            for (var i=0; i<environmentObjects.Values.Count; i++)
            {
                var obj = environmentObjects[i];
                if (entity.GetBound.Intersects(obj.GetBound))
                {
                    switch (obj.Type)
                    {
                        case GameConstants.EnvObjects.bottle_cap2:
                            break;
                        case GameConstants.EnvObjects.cube:
                            colliding = true;
                            break;
                    }
                }
            }

            return colliding;
        }
    }
}
