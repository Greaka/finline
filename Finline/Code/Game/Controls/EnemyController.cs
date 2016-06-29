using System;

using Finline.Code.Constants;
using Finline.Code.Game.Helper;
using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Finline.Code.Game.Controls
{
    using System.Threading;

    using Finline.Code.Game.Entities;

    using Timer = System.Timers.Timer;

    public class EnemyController
    {
        public delegate void Shot(Vector3 position, Vector2 direction);
        public event Shot Shoot;

        private readonly System.Timers.Timer aTimer;
        private bool shootable = true;

        private const float ShotsPerSecond = 8;

        public EnemyController()
        {
            this.aTimer = new Timer
            {
                Interval = 1000 / ShotsPerSecond,
                Enabled = true
            };
            this.aTimer.Elapsed += (sender, args) => { this.shootable = true; };
        }

        public void Update()
        {
            while (ControlsHelper.Active)
            {
                if (Math.Abs(this.aTimer.Interval - ShotsPerSecond) < 0.00001) this.aTimer.Interval = ShotsPerSecond;
                foreach (var enemy in ControlsHelper.Enemies.Values)
                {
                    enemy.Update();
                    if (enemy.shoot)
                        this.Shootroutine(enemy);
                }
                this.shootable = false;
            }
        }

        private void Shootroutine(Enemy enemy)
        {
            if (!this.shootable) return;
            this.Shoot?.Invoke(enemy.Position, (ControlsHelper.PlayerPosition - enemy.Position).get2d());
        }
    }
}
