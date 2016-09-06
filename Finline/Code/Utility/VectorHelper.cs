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
        public static Vector3 Lerp(this Vector3 from, Vector3 target, float interval)
        {
            return (1f - interval) * from + interval * target;
        }

        public static Vector3 Lerp2D(this Vector3 from, Vector3 target, float interval)
        {
            target.Z = from.Z;
            return (1f - interval) * from + interval * target;
        }

        public static Vector3 Rotate2D(this Vector3 me, float radian)
        {
            return new Vector3(me.Get2D().Rotate(radian), me.Z);
        }

        public static Vector2 Rotate(this Vector2 me, float radian)
        {
            var cosA = (float)Math.Cos(radian);
            var sinA = (float)Math.Sin(radian);

            var tmpX = me.X * cosA - me.Y * sinA;
            var tmpY = me.Y * cosA + me.X * sinA;

            me.X = tmpX;
            me.Y = tmpY;

            return me;
        }

        public static float GetAngle(this Vector2 me)
        {
            if (!(me.Length() > 0)) return 0;
            var x = (float)Math.Atan2(me.Y, me.X); // Math.Acos((double)Vector2.Dot(-Vector2.UnitY, me));
            var angle = me.X > 0 ? x : -x;
            angle += (float)Math.PI;
            return x - (float)Math.PI/2;
        }

        public static Vector2 AddPerspective(this Vector2 me)
        {
            if (!(me.Length() > 0)) return me;
            var perspective = GraphicConstants.CameraOffset.Get2D();
            perspective.Normalize();
            perspective = me.Rotate((float)Math.PI + perspective.GetAngle());
            perspective.Normalize();
            return perspective;
        }

        public static Vector2 Get2D(this Vector3 me) { return new Vector2(me.X, me.Y);}

        public static List<Vector2> Get2D(this IEnumerable<Vector3> me)
        {
            var result = new List<Vector2>();
            foreach (var vec in me)
            {
                result.Add(vec.Get2D());
            }

            return result;
        }
    }
}
