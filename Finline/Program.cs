using System;
using Finline.Code.Game;
using Finline.Code.Game.Helper;

namespace Finline
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
