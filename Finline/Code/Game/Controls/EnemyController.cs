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

        private const float ShotsPerSecond = 2;

        public EnemyController()
        {
            this.aTimer = new Timer
            {
                Interval = 1000 / ShotsPerSecond,
                Enabled = true
            };
            this.aTimer.Elapsed += (sender, args) =>
                {
                    //this.shootable = true;
                    foreach (var enemy in ControlsHelper.Enemies.Values)
                    {
                        //enemy.Update();
                        if (enemy.shoot)
                            this.Shootroutine(enemy.Position);
                    }
                };
        }

        public void Update()
        {
            foreach (var enemy in ControlsHelper.Enemies.Values)
            {
                enemy.Update();
            }
        }

        private void Shootroutine(Vector3 enemypos)
        {
            this.Shoot?.Invoke(enemypos, (ControlsHelper.PlayerPosition - enemypos).get2d());
        }
    }
}
