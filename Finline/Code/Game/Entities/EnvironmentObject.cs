// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentObject.cs" company="Acagamics e.V.">
//   APGL
// </copyright>
// <summary>
//   Defines the EnvironmentObject type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Finline.Code.Game.Entities
{
    using Finline.Code.Constants;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The environment object.
    /// </summary>
    public class EnvironmentObject : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentObject"/> class.
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
        public EnvironmentObject(ContentManager contentManager, Vector3 position, GameConstants.EnvObjects model)
        {
            this.Visible = true;
            this.Type = model;

            this.Model = contentManager.Load<Model>(model.ToString());
            this.position = position;
            this.Angle = 0;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        protected override sealed Model Model
        {
            get
            {
                return base.Model;
            }

            set
            {
                this.model = value;
                var sphere = this.Model.GetVertices().GetHull();

                switch (this.Type)
                {
                    case GameConstants.EnvObjects.wallV:
                        sphere = EnvironmentObject.VerschiebeBound(sphere, new Vector2(-1.6f, 0.2f));
                        break;
                    case GameConstants.EnvObjects.wallH:
                        sphere = EnvironmentObject.VerschiebeBound(sphere, new Vector2(0, -2f));
                        break;
                }

                this.Bound = sphere;
            }
        }

        /// <summary>
        /// Gets a value indicating whether visible.
        /// </summary>
        private bool Visible { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        private GameConstants.EnvObjects Type { get; }

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