using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Finline.Code.Game.Entities
{
    using Finline.Code.Game;

    public class HealthSystem : LivingEntity
    {
        private static int enemiesRemaining;
        private static int bossHealth;

        Sounds sounds = new Sounds();


        public void LoadContent(ContentManager content)
        {
            this.sounds.LoadContent(content);
        }

        public int GetEnemiesRemaining()
        {
            return enemiesRemaining;
        }
        public int GetBossHealth()
        {
            return bossHealth;
        }

        public void Initialize(List<Boss> bosses, List<Enemy> enemies)
        {
            enemiesRemaining = 0;
            bossHealth = 10;

            foreach (var nme in enemies)
            {
                enemiesRemaining++;
            }
            foreach (var nme in bosses)
            {
                enemiesRemaining++;
            }
        }

        public void Update(int index)
        {
            if (index == 4) bossHealth--;

            if (bossHealth == 0 || index == 3)
            {
                enemiesRemaining--;
            }
            this.sounds.SoundEffectPlay(index);
        }
    }
}
