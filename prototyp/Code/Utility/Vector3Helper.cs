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

        public static Vector3 lerp2d(this Vector3 from, Vector3 target, float interval)
        {
            target.Z = from.Z;
            return (1f - interval) * from + interval * target;
        }

        public static Vector3 rotate2d(this Vector3 me, float radian)
        {
            float cosA = (float)System.Math.Cos(radian);
            float sinA = (float)System.Math.Sin(radian);

            float tmpX = me.X * cosA - me.Y * sinA;
            float tmpY = me.Y * cosA + me.X * sinA;

            me.X = tmpX;
            me.Y = tmpY;

            return me;
        }

        public static Vector2 get2d(this Vector3 me) { return new Vector2(me.X, me.Y);}
    }
}
