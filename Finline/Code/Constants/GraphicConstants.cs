using Microsoft.Xna.Framework;

namespace Finline.Code.Constants
{
    public static class GraphicConstants
    {
        public const float FieldOfView = MathHelper.PiOver4;
        public const float NearClipPlane = 1;
        public const float FarClipPlane = 200;

        public static readonly Vector3 CameraOffset = new Vector3(-8, 0, 22);
        //public static readonly Vector3 CameraOffset = new Vector3(-1, -10, 7); //Andere Perspektive
    }
}
