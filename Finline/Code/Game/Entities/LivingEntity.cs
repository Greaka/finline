// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LivingEntity.cs" company="Acagamics">
//   Greaka
// </copyright>
// <summary>
//   The living entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Finline.Code.Game.Entities
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The living entity.
    /// </summary>
    public class LivingEntity : Entity
    {
        /// <summary>
        /// The game over.
        /// </summary>
        public delegate void GameOver();

        /// <summary>
        /// The death.
        /// </summary>
        public event GameOver Death;

        /// <summary>
        /// Gets or sets a value indicating whether dead.
        /// </summary>
        public bool Dead { get; set; }

        /// <summary>
        /// Gets or sets the model animation.
        /// </summary>
        protected Animation ModelAnimation { get; set; }

        /// <summary>
        /// Gets or sets the death animation.
        /// </summary>
        protected Animation DeathAnimation { get; set; }

        protected void Update()
        {
            if (!this.Dead)
            {
                return;
            }

            this.DeathAnimation.active = true;

            if (!this.DeathAnimation.LastModel)
            {
                return;
            }

            this.Death?.Invoke();

            return;
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
            base.Draw(viewMatrix, projectionMatrix, this.ModelAnimation.CurrentModel);
        }
    }
}
