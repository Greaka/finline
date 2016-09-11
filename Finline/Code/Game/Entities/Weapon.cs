// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Weapon.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   Defines the Weapon type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Finline.Code.Game.Entities
{
    using System;

    using Finline.Code.Game.Entities.LivingEntity;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The weapon.
    /// </summary>
    public class Weapon : Entity
    {
        /// <summary>
        /// The player.
        /// </summary>
        private readonly Player player;

        /// <summary>
        /// The death time.
        /// </summary>
        private double deathTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        public Weapon(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        protected override Model Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                this.model = value;
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="contentManager">
        /// The content manager.
        /// </param>
        public void Initialize(ContentManager contentManager)
        {
            this.Model = contentManager.Load<Model>("weapon");
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public void Update(GameTime gameTime)
        {
            this.SetViewDirection(this.player.GetViewDirection());
            var offset = new Vector3(0.5f, 1, 2.3f);
            
            if (!this.player.Dead)
            {
                this.position = this.player.Position + offset.Rotate2D(this.Angle);
                return;
            }

            if (Math.Abs(this.deathTime) < 1e-10)
            {
                this.deathTime = gameTime.TotalGameTime.TotalSeconds;
            }

            this.position.Z -= 5 * (float)Math.Pow(gameTime.TotalGameTime.TotalSeconds - this.deathTime, 2) * offset.Z;
            if (this.position.Z < this.player.Position.Z)
            {
                this.position.Z = this.player.Position.Z;
            }
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="viewMatrix">
        /// The view matrix.
        /// </param>
        /// <param name="projectionMatrix">
        /// The projection matrix.
        /// </param>
        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
          base.Draw(viewMatrix, projectionMatrix, this.Model);
        }
    }
}
