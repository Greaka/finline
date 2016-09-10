// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Shoot.cs" company="Finline OVGU">
// </copyright>
// <summary>
//   The shooting class. Handles  creating and destroying.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Controls
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Finline.Code.Game;
    using Finline.Code.Game.Entities;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// The shooting class. Handles <see cref="Projectile"/> creating and destroying.
    /// </summary>
    public class Shooting
    {
        /// <summary>
        /// The stopwatch.
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// The <see cref="ContentManager"/>.
        /// </summary>
        private readonly ContentManager content;
        
        private readonly List<Projectile> projectiles;

        private readonly Sounds sounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shooting"/> class. 
        /// Instantiates indices pool.
        /// </summary>
        /// <param name="shiny">
        /// <see cref="ContentManager"/> for loading models of Projectiles.
        /// </param>
        /// <param name="projectiles">
        /// <see cref="List{Projectile}"/> to have a reference on it in this class.
        /// </param>
        public Shooting(ContentManager shiny, List<Projectile> projectiles, Sounds sounds)
        {
            this.content = shiny;
            this.stopwatch.Restart();
            this.projectiles = projectiles;
            this.sounds = sounds;
        }

        /// <summary>
        /// Creates new <see cref="Projectile"/>, adds it to the Projectile list and binds his destructor.
        /// </summary>
        /// <param name="firedFrom">
        /// The fired From.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        public void Shoot(Entity firedFrom, Vector2 direction, int index)
        {
            var projectile = new Projectile(this.stopwatch.Elapsed, this.content, firedFrom, direction, index);
            this.projectiles.Add(projectile);
            this.sounds.SoundEffectPlay(index);
        }

        /// <summary>
        /// Update for Projectiles.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="bosses">
        /// The bosses.
        /// </param>
        /// <param name="enemies">
        /// The enemies.
        /// </param>
        /// <param name="environmentObjects">
        /// The environment Objects.
        /// </param>
        /// <param name="healthSystem">
        /// The health System.
        /// </param>
        public void Update(
            Player player,
            List<Boss> bosses,
            List<Enemy> enemies,
            List<EnvironmentObject> environmentObjects,
            HealthSystem healthSystem)
        {
            var remove = new List<Projectile>();
            foreach (var outch in this.projectiles)
            {
                outch.Update(this.stopwatch.Elapsed, player, bosses, enemies, environmentObjects, remove, healthSystem, this.sounds);
            }

            foreach (var index in remove)
            {
                this.projectiles.Remove(index);
            }
        }
    }
}
