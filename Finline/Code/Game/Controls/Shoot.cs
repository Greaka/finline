// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Shoot.cs" company="Finline OVGU">
// </copyright>
// <summary>
//   The shooting class. Handles  creating and destroying.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Controls
{
    using System.Collections.Concurrent;
    using System.Diagnostics;

    using Finline.Code.Game.Entities;
    using Finline.Code.Game.Helper;

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

        /// <summary>
        /// The pool of indices available for Projectiles.
        /// </summary>
        private readonly ConcurrentBag<int> indices = new ConcurrentBag<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Shooting"/> class. 
        /// Instantiates indices pool.
        /// </summary>
        /// <param name="shiny">
        /// <see cref="ContentManager"/> for loading models of Projectiles.
        /// </param>
        public Shooting(ContentManager shiny)
        {
            for (var i = 0; i < 1000; i++)
            {
                this.indices.Add(i);
            }

            this.content = shiny;
            this.stopwatch.Restart();
        }

        /// <summary>
        /// Creates new <see cref="Projectile"/>, adds it to the Projectile list and binds his destructor.
        /// </summary>
        public void Shoot(Vector3 position, Vector2 direction)
        {
            int index;
            this.indices.TryTake(out index);
            var projectile = new Projectile(this.stopwatch.Elapsed, this.content, index, position, direction);
            projectile.Destruct += this.AddIndex;
            ControlsHelper.Projectiles.TryAdd(index, projectile);
        }

        /// <summary>
        /// Update for Projectiles.
        /// </summary>
        public void Update()
        {
            while (ControlsHelper.Active)
            {
                foreach (var outch in ControlsHelper.Projectiles.Values)
                {
                    outch.Update(this.stopwatch.Elapsed);
                }
            }
        }

        /// <summary>
        /// Add index from destroyed <see cref="Projectile"/>.
        /// </summary>
        /// <param name="index">
        /// Index of destroyed Projectile.
        /// </param>
        private void AddIndex(int index)
        {
            Projectile projectile;
            if (ControlsHelper.Projectiles.TryRemove(index, out projectile))
            {
                this.indices.Add(index);
            }
        }
    }
}
