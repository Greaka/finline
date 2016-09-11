// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorHelper.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   Defines the VectorHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Utility
{
    using System;

    using Finline.Code.Constants;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The vector helper.
    /// </summary>
    internal static class VectorHelper
    {
/*
        /// <summary>
        /// The lerp.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="interval">
        /// The interval.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        public static Vector3 Lerp(this Vector3 from, Vector3 target, float interval)
        {
            return ((1f - interval) * from) + (interval * target);
        }
*/

/*
        /// <summary>
        /// The lerp 2 d.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="interval">
        /// The interval.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        public static Vector3 Lerp2D(this Vector3 from, Vector3 target, float interval)
        {
            target.Z = from.Z;
            return ((1f - interval) * from) + (interval * target);
        }
*/

        /// <summary>
        /// The rotate 2 d.
        /// </summary>
        /// <param name="me">
        /// The me.
        /// </param>
        /// <param name="radian">
        /// The radian.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        public static Vector3 Rotate2D(this Vector3 me, float radian)
        {
            return new Vector3(me.Get2D().Rotate(radian), me.Z);
        }

        /// <summary>
        /// The rotate.
        /// </summary>
        /// <param name="me">
        /// The me.
        /// </param>
        /// <param name="radian">
        /// The radian.
        /// </param>
        /// <returns>
        /// The <see cref="Vector2"/>.
        /// </returns>
        public static Vector2 Rotate(this Vector2 me, float radian)
        {
            var cosA = (float)Math.Cos(radian);
            var sinA = (float)Math.Sin(radian);

            var tmpX = (me.X * cosA) - (me.Y * sinA);
            var tmpY = (me.Y * cosA) + (me.X * sinA);

            me.X = tmpX;
            me.Y = tmpY;

            return me;
        }

        /// <summary>
        /// The rotate origin.
        /// </summary>
        /// <param name="vec">
        /// The vector.
        /// </param>
        /// <param name="origin">
        /// The origin.
        /// </param>
        /// <param name="angle">
        /// The angle.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        public static Vector3 RotateOrigin(this Vector3 vec, Vector3 origin, float angle)
        {
            return new Vector3(vec.Get2D().RotateOrigin(origin.Get2D(), angle), vec.Z);
        }

        /// <summary>
        /// The get angle.
        /// </summary>
        /// <param name="me">
        /// The me.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public static float GetAngle(this Vector2 me)
        {
            if (!(me.Length() > 0))
            {
                return 0;
            }

            var x = (float)Math.Atan2(me.Y, me.X); // Math.Acos((double)Vector2.Dot(-Vector2.UnitY, me));
            return x - ((float)Math.PI / 2);
        }

        /// <summary>
        /// The add perspective.
        /// </summary>
        /// <param name="me">
        /// The me.
        /// </param>
        /// <returns>
        /// The <see cref="Vector2"/>.
        /// </returns>
        public static Vector2 AddPerspective(this Vector2 me)
        {
            if (!(me.Length() > 0))
            {
                return me;
            }

            var perspective = GraphicConstants.CameraOffset.Get2D();
            perspective.Normalize();
            perspective = me.Rotate((float)Math.PI + perspective.GetAngle());
            perspective.Normalize();
            return perspective;
        }

        /// <summary>
        /// The get 2 d.
        /// </summary>
        /// <param name="me">
        /// The me.
        /// </param>
        /// <returns>
        /// The <see cref="Vector2"/>.
        /// </returns>
        public static Vector2 Get2D(this Vector3 me)
        {
            return new Vector2(me.X, me.Y);
        }

/*
        /// <summary>
        /// The get 2 d.
        /// </summary>
        /// <param name="me">
        /// The me.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public static List<Vector2> Get2D(this IEnumerable<Vector3> me)
        {
            return me.Select(vec => vec.Get2D()).ToList();
        }
*/

        /// <summary>
        /// The rotate origin.
        /// </summary>
        /// <param name="vec">
        /// The vector.
        /// </param>
        /// <param name="origin">
        /// The origin.
        /// </param>
        /// <param name="angle">
        /// The angle.
        /// </param>
        /// <returns>
        /// The <see cref="Vector2"/>.
        /// </returns>
        private static Vector2 RotateOrigin(this Vector2 vec, Vector2 origin, float angle)
        {
            var s = (float)Math.Sin(angle);
            var c = (float)Math.Cos(angle);

            // translate point back to origin:
            vec.X -= origin.X;
            vec.Y -= origin.Y;

            var xnew = (vec.X * c) - (vec.Y * s);
            var ynew = (vec.X * s) + (vec.Y * c);

            // translate point back:
            vec.X = xnew + origin.X;
            vec.Y = ynew + origin.Y;

            return vec;
        }
    }
}
