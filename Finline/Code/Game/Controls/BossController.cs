namespace Finline.Code.Game.Controls
{
    using System.Collections.Generic;
    using System.Timers;

    using Finline.Code.Game.Entities;
    using Finline.Code.Game.Entities.LivingEntity;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;

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

        public void Update(IEnumerable<Boss> bosses, Player player)
        {
            if (this.shootable != true) return;
            foreach (var boss in bosses)
            {
                if (boss.Shoot) this.Shootroutine(boss, player, 1);
            }

            this.shootable = false;
        }

        private void Shootroutine(Entity firedFrom, Player player, int index)
        {
            var direction = (player.Position - firedFrom.Position).Get2D();
            direction += player.MoveDirection * Projectile.UnitsPerSecond / direction.Length();
            this.Shoot?.Invoke(firedFrom, direction, index);
            this.Shoot?.Invoke(firedFrom, direction + new Vector2(2), index);
            this.Shoot?.Invoke(firedFrom, direction - new Vector2(2), index);
        }
    }
}
