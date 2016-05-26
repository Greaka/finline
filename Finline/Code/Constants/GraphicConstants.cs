using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finline.Code.Game.Helper;
using Microsoft.Xna.Framework;

namespace Finline.Code.Constants
{
    public static class GraphicConstants
    {
        public const float FieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
        public const float NearClipPlane = 1;
        public const float FarClipPlane = 200;

        public static readonly Vector3 CameraOffset = new Vector3(-8, -8, 6);
        public static Vector3 CameraPosition => ControlsHelper.PlayerPosition + CameraOffset;
    }
}
