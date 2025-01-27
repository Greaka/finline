﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enemy.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   Defines the Enemy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Finline.Code.Game.Entities.LivingEntity
{
    using System.Collections.Generic;

    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The enemy.
    /// </summary>
    public sealed class Enemy : LivingEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        /// <param name="contentManager">
        /// The content manager.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="environmentObjects">
        /// The environment objects.
        /// </param>
        public Enemy(ContentManager contentManager, Vector3 position, List<EnvironmentObject> environmentObjects)
        {
            this.EnvironmentObjects = environmentObjects;
            this.ModelAnimation = new Animation(3);
            this.DeathAnimation = new Animation(6, false);
            this.position = position;
            this.Angle = 0;
            var enemy = contentManager.Load<Model>("enemy");
            var enemyUnten = contentManager.Load<Model>("enemy_unten");
            var enemyUnten2 = contentManager.Load<Model>("enemy_unten2");
            var enemyUnten3 = contentManager.Load<Model>("enemy_unten3");
            var enemyUnten4 = contentManager.Load<Model>("enemy_unten4");
            var enemyUnten5 = contentManager.Load<Model>("enemy_unten5");

            this.Model = enemy;

            this.ModelAnimation.Add(enemy);
            this.ModelAnimation.Add(enemyUnten);
            this.ModelAnimation.Add(enemy);

            this.DeathAnimation.Add(enemy);
            this.DeathAnimation.Add(enemyUnten);
            this.DeathAnimation.Add(enemyUnten2);
            this.DeathAnimation.Add(enemyUnten3);
            this.DeathAnimation.Add(enemyUnten4);
            this.DeathAnimation.Add(enemyUnten5);
        }

        /// <summary>
        /// Gets a value indicating whether shoot.
        /// </summary>
        public bool Shoot { get; private set; }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="playerPosition">
        /// The player position.
        /// </param>
        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            this.Update(gameTime);

            var canSee = this.Position.CanSee(playerPosition, this.EnvironmentObjects);
            if (!canSee)
            {
                this.Shoot = false;
            }
            else
            {
                this.SetViewDirection((this.position - playerPosition).Get2D());
                this.Shoot = true;
            }
        }
    }
}
