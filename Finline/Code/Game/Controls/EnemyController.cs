using Finline.Code.Utility;

using Microsoft.Xna.Framework;

namespace Finline.Code.Game.Controls
{
    using System.Collections.Generic;

    using Finline.Code.Game.Entities;

    using Timer = System.Timers.Timer;

    public class EnemyController
    {
        public delegate void Shot(Vector3 position, Vector2 direction);
        public event Shot Shoot;

        private readonly Timer aTimer;

        private bool shootable;

        private const float ShotsPerSecond = 2;

        public EnemyController()
        {
            this.aTimer = new Timer
            {
                Interval = 1000 / ShotsPerSecond, 
                Enabled = true
            };
            this.aTimer.Elapsed += (sender, args) => { this.shootable = true; };
        }

        public void Update(IEnumerable<Enemy> enemies, Vector3 playerPosition)
        {
            if (this.shootable != true) return;
            foreach (var enemy in enemies)
            {
                if (enemy.Shoot) this.Shootroutine(enemy.Position, playerPosition);
            }

            this.shootable = false;
        }

        private void Shootroutine(Vector3 enemypos, Vector3 playerPosition)
        {
            this.Shoot?.Invoke(enemypos, (playerPosition - enemypos).Get2D());
        }
    }
}
