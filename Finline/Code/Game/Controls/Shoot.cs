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

    using Finline.Code.Game.Entities;
    using Finline.Code.Game;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// The shooting class. Handles <see cref="Projectile"/> creating and destroying.
    /// </summary>
    public class Shooting
    {
        Sounds sounds = new Sounds();

        /// <summary>
        /// The stopwatch.
        /// </summary>
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// The <see cref="ContentManager"/>.
        /// </summary>
        private readonly ContentManager content;
        
        private readonly List<Projectile> projectiles;

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
        public Shooting(ContentManager shiny, List<Projectile> projectiles)
        {
            this.content = shiny;
            this.stopwatch.Restart();
            this.projectiles = projectiles;
            sounds.LoadContent(shiny);
        }

        /// <summary>
        /// Creates new <see cref="Projectile"/>, adds it to the Projectile list and binds his destructor.
        /// </summary>
        public void Shoot(Vector3 position, Vector2 direction)
        {
            var projectile = new Projectile(this.stopwatch.Elapsed, this.content, position, direction);
            this.projectiles.Add(projectile);
            sounds.GunshotPlay();

        }

        /// <summary>
        /// Update for Projectiles.
        /// </summary>
        public void Update(List<EnvironmentObject> environmentObjects)
        {
            var remove = new List<Projectile>();
            foreach (var outch in this.projectiles)
            {
                outch.Update(this.stopwatch.Elapsed, environmentObjects, remove);
            }

            foreach (var index in remove)
            {
                this.projectiles.Remove(index);
            }
        }
    }
}
