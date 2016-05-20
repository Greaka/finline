using System.Collections.Generic;
using prototyp.Code.Constants;
using prototyp.Code.Game.Entities;

namespace prototyp.Code.Utility
{
    public static class GraphicsHelper
    {
        public static bool isColliding(this Entity entity, List<EnvironmentObject> environmentObjects)
        {
            var colliding = false;
            EnvironmentObject remove = null;
            foreach (var obj in environmentObjects)
            {
                if (entity.GetBound.Intersects(obj.GetBound))
                {
                    switch (obj.Type)
                    {
                        case GameConstants.EnvObjects.bottle_cap2:
                            remove = obj;
                            break;
                        case GameConstants.EnvObjects.cube:
                            colliding = true;
                            break;
                    }
                }
            }

            if (remove != null)
                environmentObjects.Remove(remove);

            return colliding;
        }
    }
}
