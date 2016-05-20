using System;
using prototyp.Code.Game.Helper;

namespace prototyp
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Ingame())
                game.Run();
            ControlsHelper.Active = false;
        }
    }
#endif
}
