namespace Finline.Code.Game
{
    using System.Collections.Generic;
    using System.Linq;

    using Finline.Code.Game.Entities.LivingEntity;

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
