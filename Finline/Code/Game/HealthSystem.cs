using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using System.Linq;

    using Finline.Code.Game;

    public class HealthSystem
    {
        public HealthSystem(List<Enemy> enemies, List<Boss> bosses)
        {
            this.enemies = enemies;
            this.bosses = bosses;
        }

        /// <summary>
        /// The enemies.
        /// </summary>
        private readonly List<Enemy> enemies;

        /// <summary>
        /// The bosses.
        /// </summary>
        private readonly List<Boss> bosses;

        public int GetEnemiesRemaining()
        {
            return this.enemies.Count;
        }

        public int GetBossHealth()
        {
            return this.bosses.Sum(boss => boss.Life);
        }

        public void Update(int index, Sounds sounds)
        {
            sounds.SoundEffectPlay(index);
        }
    }
}
