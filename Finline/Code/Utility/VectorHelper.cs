using System;
using Microsoft.Xna.Framework;

namespace Finline.Code.Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Finline.Code.Constants;

    internal static class VectorHelper
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
            var cosA = (float)Math.Cos(radian);
            var sinA = (float)Math.Sin(radian);

            var tmpX = me.X * cosA - me.Y * sinA;
            var tmpY = me.Y * cosA + me.X * sinA;

            me.X = tmpX;
            me.Y = tmpY;

            return me;
        }

        public static float getAngle(this Vector2 me)
        {
            if (!(me.Length() > 0)) return 0;
            var x = (float)Math.Atan2(me.Y, me.X); // Math.Acos((double)Vector2.Dot(-Vector2.UnitY, me));
            var angle = me.X > 0 ? x : -x;
            angle += (float)Math.PI;
            return x - (float)Math.PI/2;
        }

        public static Vector2 addPerspective(this Vector2 me)
        {
            if (!(me.Length() > 0)) return me;
            var perspective = GraphicConstants.CameraOffset.get2d();
            perspective.Normalize();
            perspective = me.rotate((float)Math.PI + perspective.getAngle());
            perspective.Normalize();
            return perspective;
        }

        public static Vector2 get2d(this Vector3 me) { return new Vector2(me.X, me.Y);}

        public static List<Vector2> get2d(this IEnumerable<Vector3> me)
        {
            var result = new List<Vector2>();
            foreach (var vec in me)
            {
                result.Add(vec.get2d());
            }

            return result;
        }
    }
}
