using System;
using Microsoft.Xna.Framework;

namespace prototyp.Code.Utility
{
    static class VectorHelper
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
            return new Vector3(me.get2d().rotate(radian), me.Z);
        }

        public static Vector2 rotate(this Vector2 me, float radian)
        {
            var cosA = (float)System.Math.Cos(radian);
            var sinA = (float)System.Math.Sin(radian);

            var tmpX = me.X * cosA - me.Y * sinA;
            var tmpY = me.Y * cosA + me.X * sinA;

            me.X = tmpX;
            me.Y = tmpY;

            return me;
        }

        public static float getAngle(this Vector2 me)
        {
            if (!(me.Length() > 0)) return 0;
            var x = (float)Math.Acos((double)Vector2.Dot(-Vector2.UnitY, me));
            var angle = me.X > 0 ? x : -x;
            angle += (float)Math.PI;
            return angle;
        }

        public static Vector2 get2d(this Vector3 me) { return new Vector2(me.X, me.Y);}
    }
}
