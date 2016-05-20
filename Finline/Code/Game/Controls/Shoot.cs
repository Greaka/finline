using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using prototyp.Code.Game.Entities;
using prototyp.Code.Game.Helper;

namespace prototyp.Code.Game.Controls
{
    public class Shooting
    {
        private readonly Stopwatch aStopwatch = new Stopwatch();
        private readonly ContentManager content;
        public Shooting(ContentManager shiny)
        {
            content = shiny;
            aStopwatch.Restart();
        }

        public void Shoot()
        {
            ControlsHelper.Projectiles.Add(new Projectile(aStopwatch.Elapsed, content));
        }

        public void Update()
        {
            while (ControlsHelper.Active)
            {
                foreach (var outch in ControlsHelper.Projectiles)
                {
                    outch.Update(aStopwatch.Elapsed);
                }
            }
        }
    }
}
