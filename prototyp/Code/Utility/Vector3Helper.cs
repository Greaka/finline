using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace prototyp.Code.Utility
{
    static class Vector3Helper
    {
        public static Vector3 lerp(this Vector3 from, Vector3 target, float interval)
        {
            return (1f - interval) * from + interval * target;
        }
    }
}
