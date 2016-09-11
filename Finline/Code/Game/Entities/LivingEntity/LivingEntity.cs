// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LivingEntity.cs" company="Acagamics">
//   Greaka
// </copyright>
// <summary>
//   The living entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Entities.LivingEntity
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The living entity.
    /// </summary>
    public abstract class LivingEntity : Entity
    {
        /// <summary>
        /// The game over.
        /// </summary>
        public delegate void DeathDelegate(LivingEntity me);

        /// <summary>
        /// The death.
        /// </summary>
        public event DeathDelegate Death;

        /// <summary>
        /// Gets or sets a value indicating whether dead.
        /// </summary>
        public virtual bool Dead { get; set; }

        /// <summary>
        /// Gets or sets the model animation.
        /// </summary>
        protected Animation ModelAnimation { get; set; }

        /// <summary>
        /// Gets or sets the death animation.
        /// </summary>
        protected Animation DeathAnimation { get; set; }

        protected List<EnvironmentObject> EnvironmentObjects;

        protected void Update()
        {
            if (!this.Dead)
            {
                return;
            }

            this.DeathAnimation.Active = true;

            if (!this.DeathAnimation.LastModel)
            {
                return;
            }

            Task.Factory.StartNew(
                () =>
                    {
                        Thread.Sleep(50);
                        this.Death?.Invoke(this);
                    });
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
