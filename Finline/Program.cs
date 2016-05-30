namespace Finline
{
    using System;

    using Code.GameState;

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
        private static void Main()
        {
            using (var game = new StateManager())
                game.Run();
        }
    }
#endif
}
