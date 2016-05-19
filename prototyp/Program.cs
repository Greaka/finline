using System;
using System.Threading;
using System.Threading.Tasks;
using prototyp.Code.Game.Controls;
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
            using (var game = new Game1())
                game.Run();
            ControlsHelper.Active = false;
        }
    }
#endif
}
