using Finline.Code.Utility;

using Microsoft.Xna.Framework;

namespace Finline.Code.Game.Controls
{
    using System.Collections.Generic;

    using Finline.Code.Game.Entities;

    using Timer = System.Timers.Timer;

    public class BossController
    {
        public delegate void Shot(Entity firedFrom, Vector2 direction, int index);
        public event Shot Shoot;

        private readonly Timer aTimer;

        private bool shootable;

        private const float ShotsPerSecond = 4;

        public BossController()
        {
            this.aTimer = new Timer
            {
                Interval = 1000 / ShotsPerSecond, 
                Enabled = true
            };
            this.aTimer.Elapsed += (sender, args) => { this.shootable = true; };
        }

        public void Update(IEnumerable<Boss> bosses, Vector3 playerPosition)
        {
            if (this.shootable != true) return;
            foreach (var boss in bosses)
            {
                if (boss.Shoot) this.Shootroutine(boss, playerPosition, 1);
            }

            this.shootable = false;
        }

        private void Shootroutine(Entity firedFrom, Vector3 playerPosition, int index)
        {
            this.Shoot?.Invoke(firedFrom, (playerPosition - firedFrom.Position).Get2D(), index);
            this.Shoot?.Invoke(firedFrom, (playerPosition - firedFrom.Position + new Vector3(2 , 2 , 0)).Get2D(), index);
            this.Shoot?.Invoke(firedFrom, (playerPosition - firedFrom.Position - new Vector3(2, 2, 0)).Get2D(), index);
        }
    }
}
