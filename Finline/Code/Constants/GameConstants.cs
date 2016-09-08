namespace Finline.Code.Constants
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class GameConstants
    {
        public enum EnvObjects
        {
            wallV, wallH, deskUp, chairUp, deskDown, chairDown, deskLeft, chairLeft, deskRight, chairRight, mirkopir, 
            plant, pissoir, pissoir_gedreht, whiteboard, whiteboard_gedreht, podest, 
            monitor, monitor_gedreht, rechner, rechner_gedreht, schrank, schrank_eintürig
        }

        public enum EWeaponShootMode
        {
            SingleFire, 
            Automatic
        }

        public enum EWeaponType
        {
            Pistol
        }

        public enum NonEnvObjects
        {
            poster_vader, poster_deadpool, poster_cat, poster_zombie
        }
    }
}
