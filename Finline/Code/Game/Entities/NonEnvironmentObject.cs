// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonEnvironmentObject.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   Defines the NonEnvironmentObject type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Finline.Code.Game.Entities
{
    using Finline.Code.Constants;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The non environment object.
    /// </summary>
    public sealed class NonEnvironmentObject : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonEnvironmentObject"/> class.
        /// </summary>
        /// <param name="contentManager">
        /// The content manager.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public NonEnvironmentObject(ContentManager contentManager, Vector3 position, GameConstants.NonEnvObjects model)
        {
            this.Visible = true;

            this.Model = contentManager.Load<Model>(model.ToString());
            this.position = position;
            this.Angle = 0;
        }

        /// <summary>
        /// Gets a value indicating whether visible.
        /// </summary>
        private bool Visible { get; }

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
            if (this.Visible)
            {
                base.Draw(viewMatrix, projectionMatrix);
            }
        }
    }
}